using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;
using CatechistHelper.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class AbsenceRequestController : BaseController<AbsenceRequestController>
    {
        private readonly IAbsenceRequestService _absenceRequestService;
        public AbsenceRequestController(
            IAbsenceRequestService absenceRequestService,
            ILogger<AbsenceRequestController> logger) : base(logger)
        {
            _absenceRequestService = absenceRequestService;
        }

        [HttpPost(ApiEndPointConstant.AbsenceRequest.Submit)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitAbsenceRequest([FromBody] AbsenceRequestDto absenceRequest)
        {
            Result<bool> result = await _absenceRequestService.SubmitAbsentRequest(absenceRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.AbsenceRequest.AbsenceProcess)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessAbsentRequest([FromBody] AbsenceApproveRequest absenceRequest)
        {
            Result<bool> result = await _absenceRequestService.ProcessAbsentRequest(absenceRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.AbsenceRequest.AssignCatechist)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignNewCatechist([FromBody] AssignCatechistRequest assignCatechist)
        {
            Result<bool> result = await _absenceRequestService.AssignNewCatechist(assignCatechist);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(ApiEndPointConstant.AbsenceRequest.Endpoint)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] RequestStatus status = RequestStatus.Pending, [FromQuery] Guid? cId = null)
        {
            Result<List<GetAbsentRequest>> result = await _absenceRequestService.GetAll(status, cId);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
