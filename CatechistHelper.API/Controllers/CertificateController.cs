using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Certificate;
using CatechistHelper.Domain.Dtos.Responses.Certificate;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CertificateController : BaseController<CertificateController>
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ILogger<CertificateController> logger, ICertificateService certificateService)
            : base(logger)
        {
            _certificateService = certificateService;
        }

        [HttpPost(ApiEndPointConstant.Certificate.CertificatesEndPoint)]
        [ProducesResponseType(typeof(Result<GetCertificateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCertificateRequest request)
        {
            Result<GetCertificateResponse> result = await _certificateService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Certificate.CertificateEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _certificateService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
