
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Responses.CatechistInSlot;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CatechistInSlotController : BaseController<CatechistInSlotController>
    {
        private readonly ICatechistInSlotService _catechistInSlotService;

        public CatechistInSlotController(ILogger<CatechistInSlotController> logger, ICatechistInSlotService catechistInSlotService) : base(logger)
        {
            _catechistInSlotService = catechistInSlotService;
        }

        [HttpGet(ApiEndPointConstant.CatechistInSlot.CatechistInSlotSearchEndpoint)]
        [ProducesResponseType(typeof(PagingResult<SearchCatechistResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCatechists([FromRoute] Guid id, [FromQuery] Guid excludeId, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<SearchCatechistResponse> result = await _catechistInSlotService.SearchAvailableCatechists(id, excludeId, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
