using CatechistHelper.API.Validator;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Interview;
using CatechistHelper.Domain.Dtos.Responses.Interview;
using CatechistHelper.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class InterviewController : BaseController<InterviewController>
    {
        private readonly IInterviewService _interviewService;

        public InterviewController(ILogger<InterviewController> logger, IInterviewService interviewService) : base(logger)
        {
            _interviewService = interviewService;
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Interview.InterviewsEndPoint)]
        [ProducesResponseType(typeof(Result<GetInterviewResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInterviewRequest request)
        {
            Result<GetInterviewResponse> result = await _interviewService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Interview.InterviewEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateInterviewRequest request)
        {
            Result<bool> result = await _interviewService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Interview.InterviewEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _interviewService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Catechist)]
        [HttpPut(ApiEndPointConstant.Interview.InterviewEvaluationEndPoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostEvaluation([FromRoute] Guid id, [FromBody] CreateEvaluationRequest request)
        {
            Result<bool> result = await _interviewService.PostEvaluation(id, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
