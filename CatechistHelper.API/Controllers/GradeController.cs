using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Grade;
using CatechistHelper.Domain.Dtos.Responses.Catechist;
using CatechistHelper.Domain.Dtos.Responses.Class;
using CatechistHelper.Domain.Dtos.Responses.Grade;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class GradeController : BaseController<GradeController>
    {
        private readonly IGradeService _gradeService;

        public GradeController(
            IGradeService gradeService,
            ILogger<GradeController> logger)
            : base(logger)
        {
            _gradeService = gradeService;
        }

        [HttpGet(ApiEndPointConstant.Grade.GradesEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetGradeResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetGradeResponse> result = await _gradeService.GetPagination(page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Grade.CatechistsInGradeEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetCatechistResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCatechistsByGradeId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetCatechistResponse> result = await _gradeService.GetCatechistsByGradeId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Grade.ClassesByGradeEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetClassResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClassesByGradeId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetClassResponse> result = await _gradeService.GetClassesByGradeId(id, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Grade.GradeEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetGradeResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetGradeResponse> result = await _gradeService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Grade.GradesEndpoint)]
        [ProducesResponseType(typeof(Result<GetGradeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateGradeRequest request)
        {
            Result<GetGradeResponse> result = await _gradeService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Grade.GradeEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _gradeService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
