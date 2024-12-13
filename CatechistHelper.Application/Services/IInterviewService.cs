using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Interview;
using CatechistHelper.Domain.Dtos.Responses.Interview;

namespace CatechistHelper.Application.Services
{
    public interface IInterviewService
    {
        Task<Result<GetInterviewResponse>> Create(CreateInterviewRequest request);
        Task<Result<bool>> Update(Guid id, UpdateInterviewRequest request);
        Task<Result<bool>> Delete(Guid id);
        Task<Result<bool>> PostEvaluation(Guid id, CreateEvaluationRequest request);
    }
}
