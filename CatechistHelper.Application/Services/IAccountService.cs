using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Dtos.Responses;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IAccountService
    {
        Task<PagingResult<GetAccountResponse>> GetPagination(Expression<Func<Account, bool>>? predicate, int page, int size);
        Task<Result<GetAccountResponse>> Get(Guid id);
    }
}
