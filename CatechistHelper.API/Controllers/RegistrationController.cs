using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Registration;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Registration;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;
using Microsoft.AspNetCore.Mvc;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Domain.Models;

namespace CatechistHelper.API.Controllers
{
    public class RegistrationController : BaseController<RegistrationController>
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(ILogger<RegistrationController> logger, IRegistrationService registrationService) : base(logger)
        {
            _registrationService = registrationService;
        }

        [HttpGet(ApiEndPointConstant.Registration.RegistrationsEndPoint)]
        [ProducesResponseType(typeof(PagingResult<GetRegistrationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] RegistrationFilter filter, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetRegistrationResponse> result = await _registrationService.GetPagination(filter, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Registration.InterviewsOfRegistrationEndPoint)]
        [ProducesResponseType(typeof(Result<List<GetRegistrationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInterviewOfApplication([FromRoute] Guid id)
        {
            Result<IEnumerable<GetInterviewResponse>> result = await _registrationService.GetInterviewOfApplication(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Registration.InterviewProcessesOfRegistrationEndPoint)]
        [ProducesResponseType(typeof(Result<List<GetRegistrationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInterviewProcessesOfApplication([FromRoute] Guid id)
        {
            Result<IEnumerable<GetInterviewProcessResponse>> result = await _registrationService.GetInterviewProcessOfApplication(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Registration.RegistrationEndPoint)]
        [ProducesResponseType(typeof(Result<List<GetRegistrationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetRegistrationResponse> result = await _registrationService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Registration.RegistrationsEndPoint)]
        [ProducesResponseType(typeof(Result<GetAccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateRegistrationRequest request)
        {
            Result<GetRegistrationResponse> result = await _registrationService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Registration.RegistrationEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegistrationRequest request)
        {
            Result<bool> result = await _registrationService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Registration.RegistrationEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _registrationService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
