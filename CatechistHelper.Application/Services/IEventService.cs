using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Event;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Event;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.Application.Services
{
    public interface IEventService
    {
        Task<PagingResult<GetEventResponse>> GetPagination(EventFilter? filter, int page, int size);
        Task<PagingResult<GetAccountResponse>> GetMembersByEventId(Guid id, int page, int size);
        Task<Result<GetEventResponse>> Get(Guid id);
        Task<Result<GetEventResponse>> Create(CreateEventRequest request);
        Task<Result<bool>> Update(Guid id, UpdateEventRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
