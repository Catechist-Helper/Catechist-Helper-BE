using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.MemberOfProcess;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class MemberOfProcessController : BaseController<MemberOfProcessController>
    {
        private readonly IMemberOfProcessService _memberOfProcessService;

        public MemberOfProcessController(
            IMemberOfProcessService memberOfProcessService,
            ILogger<MemberOfProcessController> logger)
            : base(logger)
        {
            _memberOfProcessService = memberOfProcessService;
        }

        [HttpPut(ApiEndPointConstant.MemberOfProcess.MemberOfProcessesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMemberToProcess([FromQuery] Guid processId, [FromBody] List<CreateMemberOfProcessRequest> request)
        {
            Result<bool> result = await _memberOfProcessService.AddMemberToProcess(processId, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}