using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface ICatechistService
    {
        Task<PagingResult<GetCatechistResponse>> GetPagination(Expression<Func<Catechist, bool>>? predicate, int page, int size);
        Task<Result<GetCatechistResponse>> Get(Guid id);
        Task<Result<GetCatechistResponse>> Create(CreateCatechistRequest request);
        Task<Result<bool>> Update(Guid id, UpdateCatechistRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
