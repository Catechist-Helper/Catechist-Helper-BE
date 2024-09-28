using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.InterviewProcess;
using CatechistHelper.Domain.Dtos.Responses.InterviewProcess;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class InterviewProcessController : BaseController<InterviewProcessController>
    {
        private readonly IInterviewProcessService _interviewProcessService;

        public InterviewProcessController(ILogger<InterviewProcessController> logger, IInterviewProcessService interviewService) : base(logger)
        {
            _interviewProcessService = interviewService;
        }

        [HttpPost(ApiEndPointConstant.InterviewProcess.InterviewProcessesEndPoint)]
        [ProducesResponseType(typeof(Result<GetInterviewProcessResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInterviewProcessRequest request)
        {
            Result<GetInterviewProcessResponse> result = await _interviewProcessService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.InterviewProcess.InterviewProcessEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateInterviewProcessRequest request)
        {
            Result<bool> result = await _interviewProcessService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.InterviewProcess.InterviewProcessEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _interviewProcessService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
