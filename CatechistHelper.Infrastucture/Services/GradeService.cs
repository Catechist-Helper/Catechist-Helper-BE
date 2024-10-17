using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.Grade;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class GradeService : BaseService<GradeService>, IGradeService
    {
        public GradeService(
            IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<GradeService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetGradeResponse>> Create(CreateGradeRequest request)
        {
            Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                predicate: m => m.Id.Equals(request.MajorId)) ?? throw new Exception(MessageConstant.Major.Fail.NotFoundMajor);
            PastoralYear pastoralYear = await _unitOfWork.GetRepository<PastoralYear>().SingleOrDefaultAsync(
                predicate: p => p.Id.Equals(request.PastoralYearId)) ?? throw new Exception(MessageConstant.PastoralYear.Fail.NotFoundPastoralYear);
            Grade grade = request.Adapt<Grade>();
            Grade result = await _unitOfWork.GetRepository<Grade>().InsertAsync(grade);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Grade.Fail.CreateGrade);
            }
            return Success(result.Adapt<GetGradeResponse>());
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
            _unitOfWork.GetRepository<Grade>().DeleteAsync(grade);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Grade.Fail.DeleteGrade);
            }
            return Success(isSuccessful);
        }

        public async Task<Result<GetGradeResponse>> Get(Guid id)
        {
            Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    include: g => g.Include(g => g.Major)
                                   .Include(g => g.PastoralYear),
                    predicate: a => a.Id.Equals(id));
            return Success(grade.Adapt<GetGradeResponse>());
        }

        public async Task<PagingResult<GetGradeResponse>> GetPagination(int page, int size)
        {
            IPaginate<Grade> grades =
                   await _unitOfWork.GetRepository<Grade>().GetPagingListAsync(
                            include: g => g.Include(g => g.Major)
                                           .Include(g => g.PastoralYear),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    grades.Adapt<IPaginate<GetGradeResponse>>(),
                    page,
                    size,
                    grades.Total);
        }
    }
}
