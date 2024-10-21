using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.TrainingList;
using CatechistHelper.Domain.Dtos.Responses.TrainingList;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class TrainingListController : BaseController<TrainingListController>
    {
        private readonly ITrainingListService _trainingListService;

        public TrainingListController(ILogger<TrainingListController> logger,
            ITrainingListService trainingListService) : base(logger)
        {
            _trainingListService = trainingListService;
        }

        [HttpGet(ApiEndPointConstant.TrainingList.TrainingListsEndpoint)]
        [ProducesResponseType(typeof(PagingResult<GetTrainingListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            PagingResult<GetTrainingListResponse> result = await _trainingListService.GetPagination(x => false, page, size);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.TrainingList.TrainingListEndpoint)]
        [ProducesResponseType(typeof(Result<List<GetTrainingListResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Result<GetTrainingListResponse> result = await _trainingListService.Get(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.TrainingList.TrainingListsEndpoint)]
        [ProducesResponseType(typeof(Result<GetTrainingListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTrainingListRequest request)
        {
            Result<GetTrainingListResponse> result = await _trainingListService.Create(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(ApiEndPointConstant.TrainingList.TrainingListEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTrainingListRequest request)
        {
            Result<bool> result = await _trainingListService.Update(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(ApiEndPointConstant.TrainingList.TrainingListEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Result<bool> result = await _trainingListService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
