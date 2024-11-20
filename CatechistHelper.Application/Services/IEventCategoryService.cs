using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.EventCategory;
using CatechistHelper.Domain.Dtos.Responses.EventCategory;

namespace CatechistHelper.Application.Services
{
    public interface IEventCategoryService
    {
        Task<PagingResult<GetEventCategoryResponse>> GetPagination(int page, int size);
        Task<Result<GetEventCategoryResponse>> Get(Guid id);
        Task<Result<GetEventCategoryResponse>> Create(CreateEventCategoryRequest request);
        Task<Result<bool>> Update(Guid id, UpdateEventCategoryRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
