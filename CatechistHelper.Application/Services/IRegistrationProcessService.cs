using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.RegistrationProcess;
using CatechistHelper.Domain.Dtos.Responses.RegistrationProcess;

namespace CatechistHelper.Application.Services
{
    public interface IRegistrationProcessService
    {
        Task<Result<GetRegistrationProcessResponse>> Create(CreateRegistrationProcessRequest request);
        Task<Result<bool>> Update(Guid id, UpdateRegistrationProcessRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
