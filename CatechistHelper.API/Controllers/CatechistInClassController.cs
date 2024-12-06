using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;
using CatechistHelper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CatechistInClassController : BaseController<CatechistInClassController>
    {
        private readonly ICatechistInClassService _catechistInClassService;

        public CatechistInClassController(ICatechistInClassService catechistInClassService,
            ILogger<CatechistInClassController> logger) : base(logger)
        {
            _catechistInClassService = catechistInClassService;
        }

        [HttpPost(ApiEndPointConstant.CatechistInClass.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCatechistToGrade([FromBody] CreateCatechistInClassRequest request)
        {
            Result<bool> result = await _catechistInClassService.AddCatechistToClass(request);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPatch(ApiEndPointConstant.CatechistInClass.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCatechistToGrade([FromBody] ReplaceCatechistInClassRequest request)
        {
            Result<bool> result = await _catechistInClassService.ReplaceCatechistInClass(request);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet(ApiEndPointConstant.CatechistInClass.CatechistInClassSearchEndpoint)]
        [ProducesResponseType(typeof(PagingResult<SearchCatechistResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCatechists([FromRoute] Guid id, [FromQuery] Guid excludeId, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<SearchCatechistResponse> result = await _catechistInClassService.SearchAvailableCatechists(id, excludeId, page, size);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
