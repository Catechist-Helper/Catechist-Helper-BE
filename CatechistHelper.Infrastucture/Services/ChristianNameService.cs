using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.ChristianName;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.ChristianName;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    internal class ChristianNameService : BaseService<ChristianNameService>, IChristianNameService
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

                IPaginate<ChristianName> christianName =
                    await _unitOfWork.GetRepository<ChristianName>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        christianName.Adapt<IPaginate<GetChristianNameResponse>>(),
                        page,
                        size,
                        christianName.Total);
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
                ChristianName account = await _unitOfWork.GetRepository<ChristianName>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                request.Adapt(account);

                _unitOfWork.GetRepository<ChristianName>().UpdateAsync(account);
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
                    predicate: a => a.Id.Equals(id));
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
                return Fail<bool>(ex.Message);
            }
        }
    }
}
