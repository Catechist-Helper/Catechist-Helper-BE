using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.SystemConfiguration;
using CatechistHelper.Domain.Dtos.Responses.SystemConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class SystemConfigurationController : BaseController<SystemConfigurationController>
    {
        private readonly ISystemConfiguration _systemConfigurationService;

        public SystemConfigurationController(ILogger<SystemConfigurationController> logger, ISystemConfiguration systemConfiguration) : base(logger)
        {
            _systemConfigurationService = systemConfiguration;
        }

        [HttpGet(ApiEndPointConstant.SystemConfiguration.SystemConfigurationsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetSystemConfigurationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetSystemConfigurationResponse> result = await _systemConfigurationService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.SystemConfiguration.SystemConfigurationEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetSystemConfigurationResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetSystemConfigurationResponse> result = await _systemConfigurationService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.SystemConfiguration.SystemConfigurationsEndpoint)]
        [ProducesResponseType(typeof(Result<GetSystemConfigurationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSystemConfigurationRequest request)
        {
            Result<GetSystemConfigurationResponse> result = await _systemConfigurationService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.SystemConfiguration.SystemConfigurationEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSystemConfigurationRequest request)
        {
            Result<bool> result = await _systemConfigurationService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.SystemConfiguration.SystemConfigurationEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _systemConfigurationService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
