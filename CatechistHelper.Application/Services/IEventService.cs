using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Event;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.Event;
using CatechistHelper.Domain.Dtos.Responses.Member;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.Application.Services
{
    public interface IEventService
    {
        Task<PagingResult<GetEventResponse>> GetPagination(EventFilter? filter, int page, int size);
        Task<PagingResult<GetMemberResponse>> GetMembersByEventId(Guid id, int page, int size);
        Task<PagingResult<GetBudgetTransactionResponse>> GetBudgetTransactionByEventId(Guid id, int page, int size);
        Task<PagingResult<GetProcessResponse>> GetProcessByEventId(Guid id, int page, int size);
        Task<PagingResult<GetParicipantInEventResponse>> GetParticipantByEventId(Guid id, int page, int size);
        Task<Result<GetEventResponse>> Get(Guid id);
        Task<Result<GetEventResponse>> Create(CreateEventRequest request);
        Task<Result<bool>> Update(Guid id, UpdateEventRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
