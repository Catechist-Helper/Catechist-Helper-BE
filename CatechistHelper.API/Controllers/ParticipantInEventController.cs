using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.ParticipantInEvent;
using CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class ParticipantInEventController : BaseController<ParticipantInEventController>
    {
        private readonly IParticipantInEventService _participantInEventService;

        public ParticipantInEventController(ILogger<ParticipantInEventController> logger, IParticipantInEventService participantInEventService) : base(logger)
        {
            _participantInEventService = participantInEventService;
        }

        [HttpGet(ApiEndPointConstant.ParticipantInEvent.ParticipantInEventEndpoint)]
        [ProducesResponseType(typeof(Result<GetParicipantInEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetParicipantInEventResponse> result = await _participantInEventService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.ParticipantInEvent.ParticipantInEventsEndpoint)]
        [ProducesResponseType(typeof(Result<GetParicipantInEventResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateParticipantInEventRequest request)
        {
            Result<GetParicipantInEventResponse> result = await _participantInEventService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.ParticipantInEvent.ParticipantInEventEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateParicipantInEventRequest request)
        {
            Result<bool> result = await _participantInEventService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.ParticipantInEvent.ParticipantInEventEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _participantInEventService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
