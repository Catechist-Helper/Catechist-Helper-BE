using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Account;
using CatechistHelper.Domain.Dtos.Responses.Account;

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
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(a => a.Role));

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
                            predicate: a => a.IsDeleted == false,
                            include: a => a.Include(x => x.Role),
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
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

        public async Task<Result<GetAccountResponse>> Create(CreateAccountRequest request)
        {
            try
            {
                Account accountFromDb = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Email.Equals(request.Email));

                if (accountFromDb != null)
                {
                    throw new Exception(MessageConstant.Account.Fail.EmailExisted);
                }

                Account account = request.Adapt<Account>();         
                account.HashedPassword = request.Password;

                Account result = await _unitOfWork.GetRepository<Account>().InsertAsync(account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.CreateAccount);
                }
                return Success(result.Adapt<GetAccountResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetAccountResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Update(Guid id, UpdateAccountRequest request)
        {
            try
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                // hash password later
                account.HashedPassword = request.Password;

                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
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

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));
                account.IsDeleted = true;
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.DeleteAccount);
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
