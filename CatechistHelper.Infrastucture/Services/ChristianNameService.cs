using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.ChristianName;
using CatechistHelper.Domain.Dtos.Responses.ChristianName;
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
    public class ChristianNameService : BaseService<ChristianNameService>, IChristianNameService
    {
        public ChristianNameService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<ChristianNameService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetChristianNameResponse>> Get(Guid id)
        {
            try
            {
                ChristianName christianName = await _unitOfWork.GetRepository<ChristianName>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                return Success(christianName.Adapt<GetChristianNameResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetChristianNameResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetChristianNameResponse>> GetPagination(Expression<Func<ChristianName, bool>>? predicate, int page, int size)
        {
            try
            {
                IPaginate<ChristianName> christianNames =
                    await _unitOfWork.GetRepository<ChristianName>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        christianNames.Adapt<IPaginate<GetChristianNameResponse>>(),
                        page,
                        size,
                        christianNames.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<GetChristianNameResponse>> Create(CreateChristianNameRequest request)
        {
            try
            {
                ChristianName christianName = request.Adapt<ChristianName>();

                ChristianName result = await _unitOfWork.GetRepository<ChristianName>().InsertAsync(christianName);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.ChristianName.Fail.CreateChristianName);
                }
                return Success(result.Adapt<GetChristianNameResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetChristianNameResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateChristianNameRequest request)
        {
            try
            {
                ChristianName christianName = await _unitOfWork.GetRepository<ChristianName>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.ChristianName.Fail.NotFoundChristianName);

                request.Adapt(christianName);

                _unitOfWork.GetRepository<ChristianName>().UpdateAsync(christianName);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.ChristianName.Fail.UpdateChristianName);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                ChristianName christianName = await _unitOfWork.GetRepository<ChristianName>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.ChristianName.Fail.NotFoundChristianName);
                _unitOfWork.GetRepository<ChristianName>().DeleteAsync(christianName);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.ChristianName.Fail.DeleteChristianName);
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
    }
}
