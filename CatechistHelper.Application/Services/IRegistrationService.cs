using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Registration;
using CatechistHelper.Domain.Dtos.Responses.Registration;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;
using CatechistHelper.Domain.Entities;
using System.Linq.Expressions;

namespace CatechistHelper.Application.Services
{
    public interface IRegistrationService
    {
        Task<PagingResult<GetRegistrationResponse>> GetPagination(Expression<Func<Registration, bool>>? predicate, int page, int size);
        Task<Result<GetRegistrationResponse>> Get(Guid id);
        Task<Result<IEnumerable<GetInterviewResponse>>> GetInterviewOfApplication(Guid id);
        Task<Result<IEnumerable<GetInterviewProcessResponse>>> GetInterviewProcessOfApplication(Guid id);
        Task<Result<GetRegistrationResponse>> Create(CreateRegistrationRequest request);
        Task<Result<bool>> Update(Guid id, UpdateRegistrationRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
