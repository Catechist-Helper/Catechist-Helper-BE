using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Class;
using CatechistHelper.Domain.Dtos.Responses.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Slot;
using CatechistHelper.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class ClassController : BaseController<ClassController>
    {
        private readonly IClassService _classService;
        public ClassController(ILogger<ClassController> logger, IClassService classService) : base(logger)
        {
            _classService = classService;
        }

        [HttpGet(ApiEndPointConstant.Class.ClassesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetClassResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] ClassFilter? filter, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetClassResponse> result = await _classService.GetPagination(filter, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Class.ClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateClass([FromBody] ClassRequest request)
        {
            Result<bool> result = await _classService.CreateClass(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Class.ClassEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClass([FromRoute] Guid id, [FromBody] ClassRequest request)
        {
            Result<bool> result = await _classService.UpdateClass(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Class.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetCatechistInClassResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCatechistInClasses([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetCatechistInClassResponse> result = await _classService.GetCatechistInClassById(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Class.SlotsOfClassEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetSlotResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSlotsByClassId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetSlotResponse> result = await _classService.GetSlotsByClassId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch(ApiEndPointConstant.Class.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClassCatechists([FromRoute] Guid id, [FromBody] CatechistInClassRequest classRequest)
        {
            Result<bool> result = await _classService.UpdateCatechistInClass(id, classRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch(ApiEndPointConstant.Class.RoomOfClassEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClassRoom([FromRoute] Guid id, [FromBody] RoomOfClassRequest request)
        {
            Result<bool> result = await _classService.UpdateClassRoom(id, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
