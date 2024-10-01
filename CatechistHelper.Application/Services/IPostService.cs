using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Post;
using CatechistHelper.Domain.Dtos.Responses.Post;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IPostService
    {
        Task<PagingResult<GetPostResponse>> GetPagination(Expression<Func<Post, bool>>? predicate, int page, int size);
        Task<Result<GetPostResponse>> Get(Guid id);
        Task<Result<GetPostResponse>> Create(CreatePostRequest request);
        Task<Result<bool>> Update(Guid id, UpdatePostRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
