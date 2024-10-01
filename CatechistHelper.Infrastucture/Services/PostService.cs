using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Post;
using CatechistHelper.Domain.Dtos.Responses.Post;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class PostService : BaseService<PostService>, IPostService
    {
        public PostService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<PostService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetPostResponse>> Create(CreatePostRequest request)
        {
            PostCategory postCategory = await _unitOfWork.GetRepository<PostCategory>().SingleOrDefaultAsync(
                    predicate: r => r.Id.Equals(request.PostCategoryId)) ?? throw new Exception(MessageConstant.PostCategory.Fail.NotFoundPostCategory); ;

            try
            {

                Post post = request.Adapt<Post>();

                Post result = await _unitOfWork.GetRepository<Post>().InsertAsync(post);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Post.Fail.CreatePost);
                }
                return Success(_mapper.Map<GetPostResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetPostResponse>(ex.Message);
            }

        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                Post post = await _unitOfWork.GetRepository<Post>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));
                post.IsDeleted = true;
                _unitOfWork.GetRepository<Post>().UpdateAsync(post);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Post.Fail.DeletePost);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }

        public async Task<Result<GetPostResponse>> Get(Guid id)
        {
            try
            {
                Post post = await _unitOfWork.GetRepository<Post>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id),
                    include: a => a.Include(a => a.Account)
                                   .Include(a => a.PostCategory));

                return Success(post.Adapt<GetPostResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetPostResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetPostResponse>> GetPagination(Expression<Func<Post, bool>>? predicate, int page, int size)
        {
            try
            {

                IPaginate<Post> posts =
                    await _unitOfWork.GetRepository<Post>()
                    .GetPagingListAsync(
                            predicate: a => a.IsDeleted == false,
                            include: a => a.Include(a => a.Account)
                                           .Include(a => a.PostCategory),
                            orderBy: a => a.OrderByDescending(x => x.CreatedAt),
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        posts.Adapt<IPaginate<GetPostResponse>>(),
                        page,
                        size,
                        posts.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdatePostRequest request)
        {
            try
            {
                Post post = await _unitOfWork.GetRepository<Post>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id));

                request.Adapt(post);

                _unitOfWork.GetRepository<Post>().UpdateAsync(post);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Post.Fail.UpdatePost);
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
