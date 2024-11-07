using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Major;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Level;
using CatechistHelper.Domain.Dtos.Responses.Major;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class MajorController : BaseController<MajorController>
    {
        private readonly IMajorService _majorService;

        public MajorController(ILogger<MajorController> logger, IMajorService majorService) : base(logger)
        {
            _majorService = majorService;
        }

        [HttpGet(ApiEndPointConstant.Major.MajorsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetMajorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetMajorResponse> result = await _majorService.GetPagination(page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Major.CatechistInMajorsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetCatechistResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQualifiedCatechistByMajorId(
                [FromRoute] Guid id, 
                [FromQuery] int page = 1, 
                [FromQuery] int size = 100,
                [FromQuery] bool excludeGradeAssigned = false
            )
        {
            PagingResult<GetCatechistResponse> result = await _majorService.GetQualifiedCatechistByMajorId(id, page, size, excludeGradeAssigned);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Major.MajorEndpoint)]
        [ProducesResponseType(typeof(Result<GetMajorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetMajorResponse> result = await _majorService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Major.MajorsEndpoint)]
        [ProducesResponseType(typeof(Result<GetMajorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMajorRequest request)
        {
            Result<GetMajorResponse> result = await _majorService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Major.MajorEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMajorRequest request)
        {
            Result<bool> result = await _majorService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpGet(ApiEndPointConstant.Major.LevelOfMajorsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetLevelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLevelOfMajor([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetLevelResponse> result = await _majorService.GetLevelOfMajor(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Major.LevelOfMajorEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLevelOfMajor([FromRoute] Guid majorId, [FromRoute] Guid levelId)
        {
            Result<bool> result = await _majorService.CreateLevelOfMajor(majorId, levelId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
