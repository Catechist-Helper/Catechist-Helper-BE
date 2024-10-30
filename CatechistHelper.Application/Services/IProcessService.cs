using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Process;
using CatechistHelper.Domain.Dtos.Responses.MemberOfProcess;
using CatechistHelper.Domain.Dtos.Responses.Process;

namespace CatechistHelper.Application.Services
{
    public interface IProcessService
    {
        Task<PagingResult<GetMemberOfProcessRepsonse>> GetMembersByProcessId(Guid id, int page, int size);
        Task<Result<GetProcessResponse>> Get(Guid id);
        Task<Result<GetProcessResponse>> Create(CreateProcessRequest request);
        Task<Result<bool>> Update(Guid id, UpdateProcessRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
