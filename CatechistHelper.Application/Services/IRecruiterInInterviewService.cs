using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.RecruiterInInterview;

namespace CatechistHelper.Application.Services
{
    public interface IRecruiterInInterviewService
    {
        Task<Result<bool>> AddRecruiterInInterview(Guid interviewId, List<CreateRecruieterInInterviewRequest> request);
    }
}
