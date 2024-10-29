using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Member;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class MemberController : BaseController<MemberController>
    {
        private readonly IMemberService _memberService;

        public MemberController(
            IMemberService memberService,
            ILogger<MemberController> logger)
            : base(logger)
        {
            _memberService = memberService;
        }

        [HttpPut(ApiEndPointConstant.Member.MemberEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMemberToEvent([FromRoute] Guid eventId, [FromBody] List<CreateMemberRequest> request)
        {
            Result<bool> result = await _memberService.AddMemberToEvent(eventId, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
