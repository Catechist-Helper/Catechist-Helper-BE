using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.ChristianName;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.ChristianName;
using CatechistHelper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class ChristianNameController : BaseController<ChristianNameController>
    {
        private readonly IChristianNameService _christianNameService;

        public ChristianNameController(ILogger<ChristianNameController> logger, IChristianNameService christianNameService) : base(logger)
        {
            _christianNameService = christianNameService;
        }

        [HttpGet(ApiEndPointConstant.ChristianName.ChristianNamesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetChristianNameResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetChristianNameResponse> result = await _christianNameService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.ChristianName.ChristianNameEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetChristianNameResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetChristianNameResponse> result = await _christianNameService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.ChristianName.ChristianNamesEndpoint)]
        [ProducesResponseType(typeof(Result<GetChristianNameResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateChristianNameRequest request)
        {
            Result<GetChristianNameResponse> result = await _christianNameService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.ChristianName.ChristianNameEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateChristianNameRequest request)
        {
            Result<bool> result = await _christianNameService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.ChristianName.ChristianNameEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _christianNameService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
