using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.InterviewProcess;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;

namespace CatechistHelper.Application.Services
{
    public interface IInterviewProcessService
    {
        Task<Result<GetInterviewProcessResponse>> Create(CreateInterviewProcessRequest request);
        Task<Result<bool>> Update(Guid id, UpdateInterviewProcessRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
