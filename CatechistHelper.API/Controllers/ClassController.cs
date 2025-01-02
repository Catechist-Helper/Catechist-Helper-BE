using CatechistHelper.API.Validator;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Class;
using CatechistHelper.Domain.Dtos.Responses.CatechistInClass;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Slot;
using CatechistHelper.Domain.Enums;
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

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.Class.ClassesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetClassResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] ClassFilter? filter, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetClassResponse> result = await _classService.GetPagination(filter, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Class.ClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateClass([FromBody] ClassRequest request)
        {
            Result<bool> result = await _classService.CreateClass(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Class.ClassEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClass([FromRoute] Guid id, [FromBody] ClassRequest request)
        {
            Result<bool> result = await _classService.UpdateClass(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Class.ClassEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _classService.DeleteClass(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin, RoleEnum.Catechist)]
        [HttpGet(ApiEndPointConstant.Class.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetCatechistInClassResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCatechistInClasses([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetCatechistInClassResponse> result = await _classService.GetCatechistInClassById(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin, RoleEnum.Catechist)]
        [HttpGet(ApiEndPointConstant.Class.SlotsOfClassEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetSlotResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSlotsByClassId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetSlotResponse> result = await _classService.GetSlotsByClassId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPatch(ApiEndPointConstant.Class.CatechistInClassesEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClassCatechists([FromRoute] Guid id, [FromBody] CatechistInClassRequest classRequest)
        {
            Result<bool> result = await _classService.UpdateCatechistInClass(id, classRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPatch(ApiEndPointConstant.Class.RoomOfClassEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClassRoom([FromRoute] Guid id, [FromBody] RoomOfClassRequest request)
        {
            Result<bool> result = await _classService.UpdateClassRoom(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [AuthorizePolicy(RoleEnum.Admin)]
        [HttpPatch(ApiEndPointConstant.Class.UpdateSlotEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCatechistInSlot([FromRoute] Guid id, [FromBody] Domain.Dtos.Requests.Slot.UpdateSlotRequest request)
        {
            Result<bool> result = await _classService.UpdateSlot(id, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }

}
