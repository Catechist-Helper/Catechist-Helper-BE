using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;

namespace CatechistHelper.Application.Services
{
    public interface IParticipantInEventService
    {
        Task<PagingResult<GetParicipantInEventResponse>> GetPagination(int page, int size);
        Task<Result<GetParicipantInEventResponse>> Get(Guid id);
        Task<Result<GetParicipantInEventResponse>> Create(CreateParticipantInEventRequest request);
        Task<Result<bool>> Update(Guid id, UpdateParicipantInEventRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
