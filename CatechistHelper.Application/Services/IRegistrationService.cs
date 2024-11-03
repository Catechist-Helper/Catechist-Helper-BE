using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Registration;
using CatechistHelper.Domain.Dtos.Responses.Registration;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Models;
using CatechistHelper.Domain.Dtos.Responses.RegistrationProcess;

namespace CatechistHelper.Application.Services
{
    public interface IRegistrationService
    {
        Task<PagingResult<GetRegistrationResponse>> GetPagination(RegistrationFilter? filter, int page, int size);
        Task<Result<GetRegistrationResponse>> Get(Guid id);
        Task<Result<IEnumerable<GetInterviewResponse>>> GetInterviewOfApplication(Guid id);
        Task<Result<IEnumerable<GetRegistrationProcessResponse>>> GetRegistrationProcessOfApplication(Guid id);
        Task<Result<GetRegistrationResponse>> Create(CreateRegistrationRequest request);
        Task<Result<bool>> Update(Guid id, UpdateRegistrationRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
