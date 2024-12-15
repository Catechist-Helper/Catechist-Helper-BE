using CatechistHelper.API.Validator;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.RecruiterInInterview;
using CatechistHelper.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruiterInInterviewController : BaseController<RecruiterInInterviewController>
    {
        private readonly IRecruiterInInterviewService _recruiterInInterviewService;

        public RecruiterInInterviewController(
            IRecruiterInInterviewService recruiterInInterviewService,
            ILogger<RecruiterInInterviewController> logger)
            : base(logger)
        {
            _recruiterInInterviewService = recruiterInInterviewService;
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.RecruiterInInterview.RecruiterInInterviewsEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRecruiterInInterview([FromRoute] Guid interviewId, [FromBody] List<CreateRecruieterInInterviewRequest> request)
        {
            Result<bool> result = await _recruiterInInterviewService.AddRecruiterInInterview(interviewId, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}