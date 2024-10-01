using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Post;
using CatechistHelper.Domain.Dtos.Responses.Post;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class PostController : BaseController<PostController>
    {
        private readonly IPostService _postService;

        public PostController(ILogger<PostController> logger, IPostService postService) : base(logger)
        {
            _postService = postService;
        }

        [HttpGet(ApiEndPointConstant.Post.PostsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetPostResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetPostResponse> result = await _postService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.Post.PostEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetPostResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetPostResponse> result = await _postService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.Post.PostsEndpoint)]
        [ProducesResponseType(typeof(Result<GetPostResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            Result<GetPostResponse> result = await _postService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.Post.PostEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostRequest request)
        {
            Result<bool> result = await _postService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.Post.PostEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _postService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
