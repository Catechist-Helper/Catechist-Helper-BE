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
    public class CatechistInGradeService : BaseService<CatechistInGradeService>, ICatechistInGrade
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
                Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                            predicate: t => t.Id.Equals(request.GradeId),
                            include: t => t.Include(t => t.CatechistInGrades)) 
                    ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
                // Find the current catechits linked to the grade
                ICollection<Guid> existingCatechistIds = await _unitOfWork.GetRepository<CatechistInGrade>().GetListAsync(
                            predicate: t => t.GradeId == request.GradeId,
                            selector: t => t.CatechisteId
                        );
                // Determine catechits to be removed (catechits that are in existingCatechistIds but not in request.Catechists.Id)
                var catechistsToRemove = await _unitOfWork.GetRepository<CatechistInGrade>().GetListAsync(
                            predicate: cig => !request.Catechists.Select(c => c.Id).Contains(cig.CatechisteId)
                        );
                _unitOfWork.GetRepository<CatechistInGrade>().DeleteRangeAsync(catechistsToRemove);
                // Get levels of major
                ICollection<Guid> levels = await _unitOfWork.GetRepository<TeachingQualification>().GetListAsync(
                            predicate: t => t.MajorId.Equals(grade.MajorId),
                            selector: t => t.LevelId
                        );
                foreach (var catechist in request.Catechists)
                {
                    Catechist catechistFromDb = await _unitOfWork.GetRepository<Catechist>().SingleOrDefaultAsync(
                            predicate: t => t.Id.Equals(catechist.Id)) ?? throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);
                    if (!levels.Contains(catechistFromDb.LevelId))
                    {
                        throw new Exception(MessageConstant.Catechist.Fail.UnqualifiedCatechist);
                    }
                    // Check if catechist is already assigned
                    if (!grade.CatechistInGrades.Any(c => c.CatechisteId == catechist.Id))
                    {
                        await _unitOfWork.GetRepository<CatechistInGrade>().InsertAsync(new CatechistInGrade
                        {
                            GradeId = request.GradeId,
                            CatechisteId = catechist.Id,
                            IsMain = catechist.IsMain
                        });
                    }
                }
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.CreateAccount);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}
