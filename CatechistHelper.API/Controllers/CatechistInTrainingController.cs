using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.CatechistInTraining;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class CatechistInTrainingController : BaseController<CatechistInTrainingController>
    {
        private readonly ICatechistInTraining _catechistInTrainingService;

        public CatechistInTrainingController(
            ICatechistInTraining catechistInTraining,
            ILogger<CatechistInTrainingController> logger)
            : base(logger)
        {
            _catechistInTrainingService = catechistInTraining;
        }

        [HttpPut(ApiEndPointConstant.CatechistInTraining.CatechistInTrainingEndpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCatechistToTrainingList([FromRoute] Guid trainingListId, [FromBody] List<CreateCatechistInTrainingRequest> request)
        {
            Result<bool> result = await _catechistInTrainingService.AddCatechistToTrainingList(trainingListId, request);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
