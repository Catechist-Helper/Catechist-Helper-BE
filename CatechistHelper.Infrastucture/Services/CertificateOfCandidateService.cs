using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CertificateOfCandidate;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCandidate;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class CertificateOfCandidateService : BaseService<CertificateOfCandidateService>, ICertificateOfCandidateService
    {
        public CertificateOfCandidateService(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<CertificateOfCandidateService> logger, IMapper mapper,
           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<GetCertificateOfCandidateResponse>> Create(CreateCertificateOfCandidateRequest request)
        {
            try
            {
                CertificateOfCandidate certificate = request.Adapt<CertificateOfCandidate>();

                CertificateOfCandidate result = await _unitOfWork.GetRepository<CertificateOfCandidate>()
                    .InsertAsync(certificate);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.CertificateOfCandidate.Fail.CreateCertificateOfCandidate);
                }
                return Success(result.Adapt<GetCertificateOfCandidateResponse>());
            }
            catch (Exception ex)
            {
                return Fail<GetCertificateOfCandidateResponse>(ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            try
            {
                CertificateOfCandidate certificate = await _unitOfWork.GetRepository<CertificateOfCandidate>()
                    .SingleOrDefaultAsync(predicate: a => a.Id.Equals(id));
                _unitOfWork.GetRepository<CertificateOfCandidate>().DeleteAsync(certificate);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.CertificateOfCandidate.Fail.DeleteCertificateOfCandidate);
                }
                return Success(isSuccessful);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.Message);
            }
        }
    }
}
