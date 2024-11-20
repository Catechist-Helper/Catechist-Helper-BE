using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Room;
using CatechistHelper.Domain.Dtos.Responses.Room;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class RoomController : BaseController<RoomController>
    {
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService) : base(logger)
        {
            _roomService = roomService;
        }

        [HttpGet(ApiEndPointConstant.Room.RoomsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetRoomResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] Guid? pastoralYearId, [FromQuery] int page = 1, [FromQuery] int size = 100, [FromQuery] bool excludeRoomAssigned = false)
        {
            PagingResult<GetRoomResponse> result = await _roomService.GetPagination(pastoralYearId, page, size, excludeRoomAssigned);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Room.RoomEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetRoomResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetRoomResponse> result = await _roomService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Room.RoomsEndpoint)]
        [ProducesResponseType(typeof(Result<GetRoomResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateRoomRequest request)
        {
            Result<GetRoomResponse> result = await _roomService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut(ApiEndPointConstant.Room.RoomEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateRoomRequest request)
        {
            Result<bool> result = await _roomService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Room.RoomEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _roomService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
