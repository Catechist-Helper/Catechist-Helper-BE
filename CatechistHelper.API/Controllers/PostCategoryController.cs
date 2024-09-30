using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.PostCategory;
using CatechistHelper.Domain.Dtos.Responses.PostCategory;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class PostCategoryController : BaseController<PostCategoryController>
    {
        private readonly IPostCategoryService _postCategoryService;

        public PostCategoryController(ILogger<PostCategoryController> logger, IPostCategoryService postCategoryService) : base(logger)
        {
            _postCategoryService = postCategoryService;
        }

        [HttpGet(ApiEndPointConstant.PostCategory.PostCategorysEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetPostCategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetPostCategoryResponse> result = await _postCategoryService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.PostCategory.PostCategoryEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetPostCategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetPostCategoryResponse> result = await _postCategoryService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost(ApiEndPointConstant.PostCategory.PostCategorysEndpoint)]
        [ProducesResponseType(typeof(Result<GetPostCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePostCategoryRequest request)
        {
            Result<GetPostCategoryResponse> result = await _postCategoryService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.PostCategory.PostCategoryEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostCategoryRequest request)
        {
            Result<bool> result = await _postCategoryService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.PostCategory.PostCategoryEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _postCategoryService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
