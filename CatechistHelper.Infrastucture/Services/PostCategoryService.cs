using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.PostCategory;
using CatechistHelper.Domain.Dtos.Responses.PostCategory;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CatechistHelper.Infrastructure.Services
{
    public class PostCategoryService : BaseService<PostCategoryService>, IPostCategoryService
    {
        public PostCategoryService(
            IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<PostCategoryService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetPostCategoryResponse>> Create(CreatePostCategoryRequest request)
        {
            try
            {
                PostCategory postCategory = request.Adapt<PostCategory>();
                PostCategory result = await _unitOfWork.GetRepository<PostCategory>().InsertAsync(postCategory);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.PostCategory.Fail.CreatePostCategory);
                }
                return Success(_mapper.Map<GetPostCategoryResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetPostCategoryResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                PostCategory postCategory = await _unitOfWork.GetRepository<PostCategory>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.PostCategory.Fail.NotFoundPostCategory);
                _unitOfWork.GetRepository<PostCategory>().DeleteAsync(postCategory);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.PostCategory.Fail.DeletePostCategory);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    // 547 is the SQL Server error code for a foreign key violation
                    return Fail<bool>(MessageConstant.Common.DeleteFail);
                }
                else
                {
                    return Fail<bool>(ex.Message);
                }
            }
        }

        public async Task<Result<GetPostCategoryResponse>> Get(Guid id)
        {
            try
            {
                PostCategory postCategory = await _unitOfWork.GetRepository<PostCategory>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.PostCategory.Fail.NotFoundPostCategory);
                return Success(postCategory.Adapt<GetPostCategoryResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetPostCategoryResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetPostCategoryResponse>> GetPagination(Expression<Func<PostCategory, bool>>? predicate, int page, int size)
        {
            try
            {
                IPaginate<PostCategory> postCategorys =
                    await _unitOfWork.GetRepository<PostCategory>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        postCategorys.Adapt<IPaginate<GetPostCategoryResponse>>(),
                        page,
                        size,
                        postCategorys.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdatePostCategoryRequest request)
        {
            try
            {
                PostCategory postCategory = await _unitOfWork.GetRepository<PostCategory>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.PostCategory.Fail.NotFoundPostCategory);
                request.Adapt(postCategory);
                _unitOfWork.GetRepository<PostCategory>().UpdateAsync(postCategory);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.PostCategory.Fail.UpdatePostCategory);
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
