using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.RoleEvent;
using CatechistHelper.Domain.Dtos.Responses.RoleEvent;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class RoleEventController : BaseController<RoleEventController>
    {
        private readonly IRoleEventService _roleEventService;

        public RoleEventController(ILogger<RoleEventController> logger, IRoleEventService roleEventService) : base(logger)
        {
            _roleEventService = roleEventService;
        }

        [HttpGet(ApiEndPointConstant.RoleEvent.RoleEventsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetRoleEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetRoleEventResponse> result = await _roleEventService.GetPagination(page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.RoleEvent.RoleEventEndpoint)]
        [ProducesResponseType(typeof(Result<GetRoleEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetRoleEventResponse> result = await _roleEventService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.RoleEvent.RoleEventsEndpoint)]
        [ProducesResponseType(typeof(Result<GetRoleEventResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoleEventRequest request)
        {
            Result<GetRoleEventResponse> result = await _roleEventService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.RoleEvent.RoleEventEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleEventRequest request)
        {
            Result<bool> result = await _roleEventService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.RoleEvent.RoleEventEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _roleEventService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}