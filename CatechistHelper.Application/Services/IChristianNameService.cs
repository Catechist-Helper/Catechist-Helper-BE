using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.ChristianName;
using CatechistHelper.Domain.Dtos.Responses.ChristianName;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IChristianNameService
    {
        Task<PagingResult<GetChristianNameResponse>> GetPagination(Expression<Func<ChristianName, bool>>? predicate, int page, int size);
        Task<Result<GetChristianNameResponse>> Get(Guid id);
        Task<Result<GetChristianNameResponse>> Create(CreateChristianNameRequest request);
        Task<Result<bool>> Update(Guid id, UpdateChristianNameRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
