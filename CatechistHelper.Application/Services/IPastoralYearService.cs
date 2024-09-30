using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.PastoralYear;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;

namespace CatechistHelper.Application.Services
{
    public interface IPastoralYearService
    {
        Task<Result<GetPastoralYearResponse>> Get(Guid id);
        Task<Result<GetPastoralYearResponse>> Create(CreatePastoralYearRequest request);
        Task<Result<bool>> Update(Guid id, UpdatePastoralYearRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
