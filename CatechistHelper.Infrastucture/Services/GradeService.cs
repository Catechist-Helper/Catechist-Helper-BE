using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Grade;
using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;
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
            GetGradeResponse grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    selector: g => new GetGradeResponse(
                                                g.Id,
                                                g.Name,
                                                g.Classes.Select(x => x.NumberOfCatechist).Sum(),
                                                g.Major.Adapt<GetMajorResponse>(),
                                                g.PastoralYear.Adapt<GetPastoralYearResponse>()
                                            ),
                    include: g => g.Include(g => g.Classes));
            return Success(grade);
        }

        public async Task<PagingResult<GetCatechistResponse>> GetCatechistsByGradeId(Guid id, int page, int size)
        {
            Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                predicate: g => g.Id == id) ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
            IPaginate<Catechist> catechists =
                   await _unitOfWork.GetRepository<CatechistInGrade>().GetPagingListAsync(
                            predicate: cig => cig.GradeId == id,
                            include: cig => cig.Include(cig => cig.Catechist),
                            selector: cig => cig.Catechist,
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    catechists.Adapt<IPaginate<GetCatechistResponse>>(),
                    page,
                    size,
                    catechists.Total);
        }

        public async Task<PagingResult<GetClassResponse>> GetClassesByGradeId(Guid id, int page, int size)
        {
            IPaginate<Class> classes =
                   await _unitOfWork.GetRepository<Class>().GetPagingListAsync(
                            predicate: c => c.GradeId == id,
                            orderBy: c => c.OrderByDescending(c => c.CreatedAt),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    classes.Adapt<IPaginate<GetClassResponse>>(),
                    page,
                    size,
                    classes.Total);
        }

        public async Task<PagingResult<GetGradeResponse>> GetPagination(int page, int size)
        {
            IPaginate<GetGradeResponse> grades =
                   await _unitOfWork.GetRepository<Grade>().GetPagingListAsync(
                            selector: g => new GetGradeResponse(
                                                g.Id, 
                                                g.Name, 
                                                g.Classes.Select(x => x.NumberOfCatechist).Sum(), 
                                                g.Major.Adapt<GetMajorResponse>(), 
                                                g.PastoralYear.Adapt<GetPastoralYearResponse>()
                                            ),
                            include: g => g.Include(g => g.Classes));
            return SuccessWithPaging(
                    grades,
                    page,
                    size,
                    grades.Total);
        }
    }
}
