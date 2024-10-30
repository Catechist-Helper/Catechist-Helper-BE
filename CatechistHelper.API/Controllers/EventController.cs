using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Event;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.BudgetTransaction;
using CatechistHelper.Domain.Dtos.Responses.Event;
using CatechistHelper.Domain.Dtos.Responses.Major;
using CatechistHelper.Domain.Dtos.Responses.Member;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.Process;
using CatechistHelper.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class EventController : BaseController<EventController>
    {
        private readonly IEventService _eventService;

        public EventController(ILogger<EventController> logger, IEventService eventService) : base(logger)
        {
            _eventService = eventService;
        }

        [HttpGet(ApiEndPointConstant.Event.EventsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] EventFilter? filter, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetEventResponse> result = await _eventService.GetPagination(filter, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Event.MemberInEventEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetMemberResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMemberInEvent([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetMemberResponse> result = await _eventService.GetMembersByEventId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Event.BudgetTransactionInEventEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetBudgetTransactionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBudgetTransactionByEventId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetBudgetTransactionResponse> result = await _eventService.GetBudgetTransactionByEventId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Event.ProcessInEventEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetProcessResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProcessByEventId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetProcessResponse> result = await _eventService.GetProcessByEventId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Event.ParticipantInEventEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetParicipantInEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParticipantByEventId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetParicipantInEventResponse> result = await _eventService.GetParticipantByEventId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Event.EventEndpoint)]
        [ProducesResponseType(typeof(Result<GetEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetEventResponse> result = await _eventService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Event.EventsEndpoint)]
        [ProducesResponseType(typeof(Result<GetMajorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            Result<GetEventResponse> result = await _eventService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Event.EventEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEventRequest request)
        {
            Result<bool> result = await _eventService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Event.EventEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _eventService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
