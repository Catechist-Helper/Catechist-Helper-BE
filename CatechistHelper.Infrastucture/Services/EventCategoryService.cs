using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.EventCategory;
using CatechistHelper.Domain.Dtos.Responses.EventCategory;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Pagination;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class EventCategoryService : BaseService<EventCategoryService>, IEventCategoryService
    {
        public EventCategoryService(
            IUnitOfWork<ApplicationDbContext> unitOfWork, 
            ILogger<EventCategoryService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetEventCategoryResponse>> Create(CreateEventCategoryRequest request)
        {
            try
            {
                EventCategory eventCategory = request.Adapt<EventCategory>();
                EventCategory result = await _unitOfWork.GetRepository<EventCategory>().InsertAsync(eventCategory);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.EventCategory.Fail.CreateEventCategory);
                }
                return Success(_mapper.Map<GetEventCategoryResponse>(result));
            }
            catch (Exception ex)
            {
                return Fail<GetEventCategoryResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                EventCategory eventCategory = await _unitOfWork.GetRepository<EventCategory>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.EventCategory.Fail.NotFoundEventCategory);
                _unitOfWork.GetRepository<EventCategory>().DeleteAsync(eventCategory);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.EventCategory.Fail.DeleteEventCategory);
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

        public async Task<Result<GetEventCategoryResponse>> Get(Guid id)
        {
            try
            {
                EventCategory eventCategory = await _unitOfWork.GetRepository<EventCategory>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.EventCategory.Fail.NotFoundEventCategory);
                return Success(eventCategory.Adapt<GetEventCategoryResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest<GetEventCategoryResponse>(ex.Message);
            }
        }

        public async Task<PagingResult<GetEventCategoryResponse>> GetPagination(int page, int size)
        {
            try
            {
                IPaginate<EventCategory> eventCategories =
                    await _unitOfWork.GetRepository<EventCategory>()
                    .GetPagingListAsync(
                            page: page,
                            size: size
                        );
                return SuccessWithPaging(
                        eventCategories.Adapt<IPaginate<GetEventCategoryResponse>>(),
                        page,
                        size,
                        eventCategories.Total);
            }
            catch (Exception ex)
            {
            }
            return null!;
        }

        public async Task<Result<bool>> Update(Guid id, UpdateEventCategoryRequest request)
        {
            try
            {
                EventCategory eventCategory = await _unitOfWork.GetRepository<EventCategory>().SingleOrDefaultAsync(
                    predicate: a => a.Id.Equals(id)) ?? throw new Exception(MessageConstant.EventCategory.Fail.NotFoundEventCategory);
                request.Adapt(eventCategory);
                _unitOfWork.GetRepository<EventCategory>().UpdateAsync(eventCategory);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.EventCategory.Fail.NotFoundEventCategory);
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
