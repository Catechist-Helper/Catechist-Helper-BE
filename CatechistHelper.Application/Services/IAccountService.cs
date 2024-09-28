using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;
using CatechistHelper.Domain.Dtos.Requests.Account;
using CatechistHelper.Domain.Dtos.Responses.Account;

namespace CatechistHelper.Application.Services
{
    public interface IAccountService
    {
        Task<PagingResult<GetAccountResponse>> GetPagination(Expression<Func<Account, bool>>? predicate, int page, int size);
        Task<Result<GetAccountResponse>> Get(Guid id);
        Task<Result<GetAccountResponse>> Create(CreateAccountRequest request);
        Task<Result<bool>> Update(Guid id, UpdateAccountRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
