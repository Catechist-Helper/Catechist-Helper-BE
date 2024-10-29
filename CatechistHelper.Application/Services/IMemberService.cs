using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Member;

namespace CatechistHelper.Application.Services
{
    public interface IMemberService
    {
        Task<Result<bool>> AddMemberToEvent(Guid eventId, List<CreateMemberRequest> request);
    }
}
