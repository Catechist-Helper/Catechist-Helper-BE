using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CertificateOfCatechist;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CatechistHelper.Infrastructure.Services
{
    public class CertificateOfCatechistService : BaseService<CertificateOfCatechistService>, ICertificateOfCatechistService
    {
        public CertificateOfCatechistService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<CertificateOfCatechistService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Result<bool>> Create(CreateCertificateOfCatechistRequest request)
        {
            try
            {
                Catechist catechist = await _unitOfWork.GetRepository<Catechist>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(request.CatechistId)) ?? throw new Exception(MessageConstant.Catechist.Fail.NotFoundCatechist);
                Certificate certificate = await _unitOfWork.GetRepository<Certificate>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(request.CertificateId)) ?? throw new Exception(MessageConstant.Certificate.Fail.NotFoundCertificate);
                CertificateOfCatechist certificateOfCatechist = request.Adapt<CertificateOfCatechist>();
                CertificateOfCatechist result = await _unitOfWork.GetRepository<CertificateOfCatechist>().InsertAsync(certificateOfCatechist);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new Exception(MessageConstant.Account.Fail.CreateAccount);
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
