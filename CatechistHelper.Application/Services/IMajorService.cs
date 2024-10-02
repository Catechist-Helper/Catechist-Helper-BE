using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Major;
using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IMajorService
    {
        Task<PagingResult<GetMajorResponse>> GetPagination(Expression<Func<Major, bool>>? predicate, int page, int size);
        Task<Result<GetMajorResponse>> Get(Guid id);
        Task<Result<GetMajorResponse>> Create(CreateMajorRequest request);
        Task<Result<bool>> Update(Guid id, UpdateMajorRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
