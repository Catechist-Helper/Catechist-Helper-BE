using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Domain.Dtos.Responses;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetAccountResponse>> Get(Guid id)
        {
            try
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));

                return Success(account.Adapt<GetAccountResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetAccountResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetAccountResponse>> GetPagination(Expression<Func<Account, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<Account> accounts =
                    await _unitOfWork.GetRepository<Account>()
                    .GetPagingListAsync(
                            predicate: x => x.IsDeleted == false,
                            include: x => x.Include(x => x.Role),
                            orderBy: x => x.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        accounts.Adapt<IPaginate<GetAccountResponse>>(),
                        page,
                        size,
                        accounts.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

    }
}
