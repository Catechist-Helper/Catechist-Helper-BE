using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Level;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Dtos.Responses.Major;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class LevelController : BaseController<LevelController>
    {
        private readonly ILevelService _levelService;
        public LevelController(ILogger<LevelController> logger, ILevelService levelService) : base(logger)
        {
            _levelService = levelService;
        }

        [HttpGet(ApiEndPointConstant.Level.LevelsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetLevelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetLevelResponse> result = await _levelService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Level.LevelEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetLevelResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetLevelResponse> result = await _levelService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Level.LevelsEndpoint)]
        [ProducesResponseType(typeof(Result<GetLevelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateLevelRequest request)
        {
            Result<GetLevelResponse> result = await _levelService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Level.LevelEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateLevelRequest request)
        {
            Result<bool> result = await _levelService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Level.LevelEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _levelService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Level.MajorOfLevelsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetMajorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLevelOfMajor([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetMajorResponse> result = await _levelService.GetMajorOfLevel(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
