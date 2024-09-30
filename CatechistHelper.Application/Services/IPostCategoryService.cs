using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.PostCategory;
using CatechistHelper.Domain.Dtos.Responses.PostCategory;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IPostCategoryService
    {
        Task<PagingResult<GetPostCategoryResponse>> GetPagination(Expression<Func<PostCategory, bool>>? predicate, int page, int size);
        Task<Result<GetPostCategoryResponse>> Get(Guid id);
        Task<Result<GetPostCategoryResponse>> Create(CreatePostCategoryRequest request);
        Task<Result<bool>> Update(Guid id, UpdatePostCategoryRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
