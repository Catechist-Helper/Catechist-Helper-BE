using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CertificateOfCandidate;
using CatechistHelper.Domain.Dtos.Responses.CertificateOfCandidate;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CertificateOfCandidateController : BaseController<CertificateOfCandidateController>
    {
        private readonly ICertificateOfCandidateService _certificateOfCandidateService;

        public CertificateOfCandidateController(ILogger<CertificateOfCandidateController> logger, ICertificateOfCandidateService certificateOfCandidateService) 
            : base(logger)
        {
            _certificateOfCandidateService = certificateOfCandidateService;
        }

        [HttpPost(ApiEndPointConstant.CertificateOfCandidate.CertificatesOfCandidateEndPoint)]
        [ProducesResponseType(typeof(Result<GetCertificateOfCandidateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCertificateOfCandidateRequest request)
        {
            Result<GetCertificateOfCandidateResponse> result = await _certificateOfCandidateService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.CertificateOfCandidate.CertificateOfCandidateEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _certificateOfCandidateService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
