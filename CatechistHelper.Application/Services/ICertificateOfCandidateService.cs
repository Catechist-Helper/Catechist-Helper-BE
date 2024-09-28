using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.CertificateOfCandidate;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCandidate;

namespace CatechistHelper.Application.Services
{
    public interface ICertificateOfCandidateService
    {
        Task<Result<GetCertificateOfCandidateResponse>> Create(CreateCertificateOfCandidateRequest request);
        Task<Result<bool>> Delete(Guid id);
    }
}
