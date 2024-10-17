using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Level;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface ILevelService
    {
        Task<PagingResult<GetLevelResponse>> GetPagination(Expression<Func<Level, bool>>? predicate, int page, int size);
        Task<Result<GetLevelResponse>> Get(Guid id);
        Task<Result<GetLevelResponse>> Create(CreateLevelRequest request);
        Task<Result<bool>> Delete(Guid id);
        Task<Result<bool>> Update(Guid id, UpdateLevelRequest request);
    }
}
