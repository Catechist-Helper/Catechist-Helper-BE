using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CertificateOfCatechist;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CertificateOfCatechistController : BaseController<CertificateOfCatechistController>
    {
        private readonly ICertificateOfCatechistService _certificateOfCatechistService;

        public CertificateOfCatechistController(
            ICertificateOfCatechistService certificateOfCatechistService,
            ILogger<CertificateOfCatechistController> logger) 
            : base(logger)
        {
            _certificateOfCatechistService = certificateOfCatechistService;
        }

        [HttpPost(ApiEndPointConstant.CertificateOfCatechist.CertificateOfCatechistsEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCertificateOfCatechistRequest request)
        {
            Result<bool> result = await _certificateOfCatechistService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
