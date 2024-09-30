using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.PastoralYear;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.PastoralYear;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class PastoralYearController : BaseController<PastoralYearController>
    {
        private readonly IPastoralYearService _pastoralYearService;
        public PastoralYearController(ILogger<PastoralYearController> logger, IPastoralYearService pastoralYearService) : base(logger)
        {
            _pastoralYearService = pastoralYearService;
        }

        [HttpGet(ApiEndPointConstant.PastoralYear.PastoralYearsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetAccountResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetPastoralYearResponse> result = await _pastoralYearService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.PastoralYear.PastoralYearEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetPastoralYearResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetPastoralYearResponse> result = await _pastoralYearService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.PastoralYear.PastoralYearsEndpoint)]
        [ProducesResponseType(typeof(Result<GetPastoralYearResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePastoralYearRequest request)
        {
            Result<GetPastoralYearResponse> result = await _pastoralYearService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.PastoralYear.PastoralYearEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePastoralYearRequest request)
        {
            Result<bool> result = await _pastoralYearService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.PastoralYear.PastoralYearEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _pastoralYearService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
