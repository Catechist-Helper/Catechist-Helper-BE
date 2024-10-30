using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.MemberOfProcess;

namespace CatechistHelper.Application.Services
{
    public interface IMemberOfProcessService
    {
        Task<Result<bool>> AddMemberToProcess(Guid processId, List<CreateMemberOfProcessRequest> request);
    }
}
