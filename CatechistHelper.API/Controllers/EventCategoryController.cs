using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.EventCategory;
using CatechistHelper.Domain.Dtos.Requests.PostCategory;
using CatechistHelper.Domain.Dtos.Responses.EventCategory;
using CatechistHelper.Domain.Dtos.Responses.PostCategory;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class EventCategoryController : BaseController<EventCategoryController>
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoryController(ILogger<EventCategoryController> logger, IEventCategoryService eventCategoryService) : base(logger)
        {
            _eventCategoryService = eventCategoryService;
        }

        [HttpGet(ApiEndPointConstant.EventCategory.EventCategoriesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetEventCategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetEventCategoryResponse> result = await _eventCategoryService.GetPagination(page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.EventCategory.EventCategoryEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetEventCategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetEventCategoryResponse> result = await _eventCategoryService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.EventCategory.EventCategoriesEndpoint)]
        [ProducesResponseType(typeof(Result<GetEventCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEventCategoryRequest request)
        {
            Result<GetEventCategoryResponse> result = await _eventCategoryService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.EventCategory.EventCategoryEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEventCategoryRequest request)
        {
            Result<bool> result = await _eventCategoryService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.EventCategory.EventCategoryEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _eventCategoryService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
