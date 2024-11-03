using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.RegistrationProcess;
using CatechistHelper.Domain.Dtos.Responses.RegistrationProcess;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class RegistrationProcessController : BaseController<RegistrationProcessController>
    {
        private readonly IRegistrationProcessService _registrationProcessService;

        public RegistrationProcessController(ILogger<RegistrationProcessController> logger, IRegistrationProcessService registrationProcessService) : base(logger)
        {
            _registrationProcessService = registrationProcessService;
        }

        [HttpPost(ApiEndPointConstant.RegistrationProcess.RegistrationProcessesEndPoint)]
        [ProducesResponseType(typeof(Result<GetRegistrationProcessResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRegistrationProcessRequest request)
        {
            Result<GetRegistrationProcessResponse> result = await _registrationProcessService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.RegistrationProcess.RegistrationProcessEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegistrationProcessRequest request)
        {
            Result<bool> result = await _registrationProcessService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.RegistrationProcess.RegistrationProcessEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _registrationProcessService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
