using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.Application.Services
{
    public interface IBudgetTransactionService
    {
        Task<PagingResult<GetBudgetTransactionResponse>> GetPagination(BudgetTransactionFilter? filter, int page, int size);
        Task<Result<GetBudgetTransactionResponse>> Get(Guid id);
        Task<Result<GetBudgetTransactionResponse>> Create(CreateBudgetTransactionRequest request);
        Task<Result<bool>> Update(Guid id, UpdateBudgetTransactionRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
