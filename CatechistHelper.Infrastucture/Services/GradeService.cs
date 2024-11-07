using CatechistHelper.API.Utils;
using CatechistHelper.Application.Extensions;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.CatechistInGrade;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Grade;
using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Models;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq.Expressions;

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
            await CheckRestrictionDateAsync();
            Major major = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(
                predicate: m => m.Id.Equals(request.MajorId)) ?? throw new Exception(MessageConstant.Major.Fail.NotFoundMajor);
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
            try
            {
                await CheckRestrictionDateAsync();
                Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    predicate: g => g.Id.Equals(id)) ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
                _unitOfWork.GetRepository<Grade>().DeleteAsync(grade);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Grade.Fail.DeleteGrade);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    // 547 is the SQL Server error code for a foreign key violation
                    return Fail<bool>(MessageConstant.Common.DeleteFail);
                }
                else
                {
                    return Fail<bool>(ex.Message);
                }
            }     
        }

        public async Task<Result<GetGradeResponse>> Get(Guid id)
        {
            GetGradeResponse grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    predicate: g => g.Id.Equals(id),
                    selector: g => new GetGradeResponse(
                                                g.Id,
                                                g.Name,
                                                g.Classes.Select(x => x.NumberOfCatechist).Sum(),
                                                g.Major.Adapt<GetMajorResponse>()
                                            ),
                    include: g => g.Include(g => g.Classes));
            return Success(grade);
        }

        public async Task<PagingResult<GetClassResponse>> GetClassesByGradeId(Guid id, Guid? pastoralYearId, int page, int size)
        {
            Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                predicate: g => g.Id == id) ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
            IPaginate<Class> classes =
                   await _unitOfWork.GetRepository<Class>().GetPagingListAsync(
                            predicate: BuildGetClassesByGradeIdQuery(id, pastoralYearId),
                            orderBy: c => c.OrderByDescending(c => c.CreatedAt),
                            page: page,
                            size: size);
            return SuccessWithPaging(
                    classes.Adapt<IPaginate<GetClassResponse>>(),
                    page,
                    size,
                    classes.Total);
        }

        private Expression<Func<Class, bool>> BuildGetClassesByGradeIdQuery(Guid id, Guid? pastoralYearId)
        {
            Expression<Func<Class, bool>> filterQuery = c => c.GradeId == id;
            if (pastoralYearId != null)
            {
                filterQuery = filterQuery.AndAlso(c => c.PastoralYearId == pastoralYearId);
            }
            return filterQuery;
        }

        public async Task<PagingResult<GetGradeResponse>> GetPagination(GradeFilter filter, int page, int size)
        {
            IPaginate<GetGradeResponse> grades =
                   await _unitOfWork.GetRepository<Grade>().GetPagingListAsync(
                            predicate: g => (!filter.MajorId.HasValue || g.MajorId.Equals(filter.MajorId)),
                            selector: g => new GetGradeResponse(
                                                g.Id, 
                                                g.Name, 
                                                g.Classes.Select(x => x.NumberOfCatechist).Sum(), 
                                                g.Major.Adapt<GetMajorResponse>()
                                            ),
                            include: g => g.Include(g => g.Classes));
            return SuccessWithPaging(
                    grades,
                    page,
                    size,
                    grades.Total);
        }

        public async Task<PagingResult<GetCatechistInGradeResponse>> GetCatechistsByGradeId(
            Guid id,
            int page, 
            int size, 
            bool excludeClassAssigned = false)
        {
            try
            {
                Grade grade = await _unitOfWork.GetRepository<Grade>().SingleOrDefaultAsync(
                    predicate: g => g.Id == id) ?? throw new Exception(MessageConstant.Grade.Fail.NotFoundGrade);
                ICollection<Guid> assignedCatechistInClassIds = new List<Guid>();
                if (excludeClassAssigned)
                {
                    assignedCatechistInClassIds = await _unitOfWork.GetRepository<CatechistInClass>()
                        .GetListAsync(
                            predicate: cic => cic.Class.GradeId == id,
                            selector: cic => cic.CatechistId);
                }
                IPaginate<CatechistInGrade> catechists = await _unitOfWork.GetRepository<CatechistInGrade>()
                    .GetPagingListAsync(
                        predicate: cig => cig.GradeId == id
                                          && cig.Catechist.IsDeleted == false       
                                          && cig.Catechist.IsTeaching == true   
                                          && (!excludeClassAssigned || !assignedCatechistInClassIds.Contains(cig.CatechisteId)),
                        include: c => c.Include(n => n.Catechist.ChristianName)
                                .Include(n => n.Catechist.Level)
                                .Include(n => n.Catechist.Account)
                                .Include(n => n.Catechist.Certificates),
                        page: page,
                        size: size
                    );
                return SuccessWithPaging(
                    catechists.Adapt<IPaginate<GetCatechistInGradeResponse>>(),
                    page,
                    size,
                    catechists.Total
                );
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        private async Task CheckRestrictionDateAsync()
        {
            var configEntry = await _unitOfWork.GetRepository<SystemConfiguration>()
                .SingleOrDefaultAsync(predicate: sc => sc.Key == EnumUtil.GetDescriptionFromEnum(SystemConfigurationEnum.RestrictedDateManagingCatechism));

            if (configEntry == null || !DateTime.TryParseExact(configEntry.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var restrictionDate))
            {
                throw new Exception(MessageConstant.Common.RestrictedDateManagingCatechism);
            }

            if (DateTime.Now >= restrictionDate)
            {
                throw new Exception(MessageConstant.Common.RestrictedDateManagingCatechism);
            }
        }
    }
}
