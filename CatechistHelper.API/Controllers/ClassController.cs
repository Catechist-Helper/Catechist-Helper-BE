using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Responses.Class;
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
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetClassResponse> result = await _classService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
