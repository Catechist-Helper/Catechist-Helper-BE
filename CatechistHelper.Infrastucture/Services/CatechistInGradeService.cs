using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInGrade;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class CatechistInGradeService : BaseService<CatechistInGradeService>, ICatechistInGradeService
    {
        public CatechistInGradeService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<CatechistInGradeService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> AddCatechistToGrade(CreateCatechistInGradeRequest request)
        {
            try
            {
                var grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    predicate: g => g.Id == request.GradeId,
                    include: g => g.Include(g => g.CatechistInGrades)
                ) ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
                var levels = await _unitOfWork.GetRepository<TeachingQualification>().GetListAsync(
                    predicate: t => t.MajorId == grade.MajorId,
                    selector: t => t.LevelId
                );
                var requestedCatechists = await _unitOfWork.GetRepository<Catechist>().GetListAsync(
                    predicate: c => request.CatechistIds.Contains(c.Id)
                );
                if (requestedCatechists.Count != request.CatechistIds.Count)
                {
                    throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);
                }
                // Filter out unqualified catechists
                var unqualifiedCatechists = requestedCatechists.Where(c => !levels.Contains(c.LevelId)).ToList();
                if (unqualifiedCatechists.Any())
                {
                    throw new Exception(MessageConstant.Catechist.Fail.UnqualifiedCatechist);
                }
                // Remove catechists not in the request
                var catechistsToRemove = grade.CatechistInGrades.Where(cig => !request.CatechistIds.Contains(cig.CatechisteId)).ToList();
                if (catechistsToRemove.Any())
                {
                    _unitOfWork.GetRepository<CatechistInGrade>().DeleteRangeAsync(catechistsToRemove);
                }
                foreach (var catechist in requestedCatechists)
                {
                    var isMain = catechist.Id == request.MainCatechistId;
                    var existingCatechistInGrade = grade.CatechistInGrades.FirstOrDefault(cig => cig.CatechisteId == catechist.Id);
                    if (existingCatechistInGrade != null)
                    {
                        if (existingCatechistInGrade.IsMain != isMain)
                        {
                            existingCatechistInGrade.IsMain = isMain;
                            _unitOfWork.GetRepository<CatechistInGrade>().UpdateAsync(existingCatechistInGrade);
                        }
                    }
                    else
                    {
                        await _unitOfWork.GetRepository<CatechistInGrade>().InsertAsync(new CatechistInGrade
                        {
                            GradeId = request.GradeId,
                            CatechisteId = catechist.Id,
                            IsMain = isMain
                        });
                    }
                }
                await _unitOfWork.CommitAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}
