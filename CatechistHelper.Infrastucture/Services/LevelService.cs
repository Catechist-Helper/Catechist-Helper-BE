using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Level;
using CatechistHelper.Domain.Dtos.Responses.Certificate;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class LevelService : BaseService<LevelService>, ILevelService
    {
        public LevelService(
            IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<LevelService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetLevelResponse>> Create(CreateLevelRequest request)
        {
            try
            {
                var result = await CreateAsync(request);
                return Success(result.Adapt<GetLevelResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetLevelResponse>(ex.Message);
            }
        }

        public async Task<Level> CreateAsync(CreateLevelRequest request)
        {
            var level = request.Adapt<Level>();
            var result = await _unitOfWork.GetRepository<Level>().InsertAsync(level);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception(MessageConstant.Level.Fail.CreateLevel);
            }
            return result;
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                var level = await GetById(id);
                _unitOfWork.GetRepository<Level>().DeleteAsync(level);
                return Success(await _unitOfWork.CommitAsync() > 0);
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

        public async Task<Result<GetLevelResponse>> Get(Guid id)
        {
            var result = await GetById(id);
            return Success(result.Adapt<GetLevelResponse>());
        }

        public async Task<Level> GetById(Guid id)
        {
            var level = await _unitOfWork.GetRepository<Level>()
                .SingleOrDefaultAsync(predicate: l => l.Id.Equals(id));
            ArgumentNullException.ThrowIfNull(nameof(id));
            return level;
        }

        public async Task<IPaginate<Level>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository<Level>()
                     .GetPagingListAsync(
                            orderBy: l => l.OrderBy(l => l.HierarchyLevel)
                     );
        }

        public async Task<PagingResult<GetLevelResponse>> GetPagination(Expression<Func<Level, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<Level> accounts =
                    await _unitOfWork.GetRepository<Level>()
                    .GetPagingListAsync(
                            orderBy: l => l.OrderByDescending(l => l.HierarchyLevel),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        accounts.Adapt<IPaginate<GetLevelResponse>>(),
                        page,
                        size,
                        accounts.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateLevelRequest request)
        {
            try
            {
                Level level = await GetById(id);
                request.Adapt(level);
                _unitOfWork.GetRepository<Level>().UpdateAsync(level);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Level.Fail.UpdateLevel);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<PagingResult<GetMajorResponse>> GetMajorOfLevel(Guid id, int page, int size)
        {
            try
            {
                Level level = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(id)) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                IPaginate<Major> majors = await _unitOfWork.GetRepository<TeachingQualification>().GetPagingListAsync(
                                predicate: tq => tq.LevelId.Equals(id),
                                selector: tq => tq.Major,
                                page: page,
                                size: size
                            ); ;
                return SuccessWithPaging(
                        majors.Adapt<IPaginate<GetMajorResponse>>(),
                        page,
                        size,
                        majors.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<PagingResult<GetCertificateResponse>> GetCertificateOfLevel(Guid id, int page, int size)
        {
            try
            {
                Level level = await _unitOfWork.GetRepository<Level>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(id)) ?? throw new Exception(MessageConstant.Level.Fail.NotFoundLevel);
                IPaginate<Certificate> certificates = await _unitOfWork.GetRepository<Certificate>().GetPagingListAsync(
                                predicate: c => c.LevelId == id,
                                page: page,
                                size: size
                            ); ;
                return SuccessWithPaging(
                        certificates.Adapt<IPaginate<GetCertificateResponse>>(),
                        page,
                        size,
                        certificates.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }
    }
}
