using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Process;
using CatechistHelper.Domain.Dtos.Responses.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.MemberOfProcess;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class ProcessController : BaseController<ProcessController>
    {
        private readonly IProcessService _processService;

        public ProcessController(ILogger<ProcessController> logger, IProcessService processService) : base(logger)
        {
            _processService = processService;
        }

        [HttpGet(ApiEndPointConstant.MemberOfProcess.MemberOfProcessEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetMemberOfProcessRepsonse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCatechistInClasses([FromQuery] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetMemberOfProcessRepsonse> result = await _processService.GetMembersByProcessId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Process.ProcessEndpoint)]
        [ProducesResponseType(typeof(Result<GetProcessResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetProcessResponse> result = await _processService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Process.ProcessesEndpoint)]
        [ProducesResponseType(typeof(Result<GetProcessResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProcessRequest request)
        {
            Result<GetProcessResponse> result = await _processService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Process.ProcessEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProcessRequest request)
        {
            Result<bool> result = await _processService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Process.ProcessEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _processService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
