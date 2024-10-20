using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CertificateOfCatechist;

namespace CatechistHelper.Application.Services
{
    public interface ICertificateOfCatechistService
    {
        Task<Result<bool>> Create(CreateCertificateOfCatechistRequest request);
    }
}