
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Requests.LeaveRequest;
using CatechistHelper.Domain.Dtos.Responses.AbsenceRequest;
using CatechistHelper.Domain.Dtos.Responses.LeaveRequest;
using CatechistHelper.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class LeaveRequestController : BaseController<LeaveRequestController>
    {
        private readonly ILeaveRequestService _leaveRequestService;
        public LeaveRequestController(ILogger<LeaveRequestController> logger, ILeaveRequestService leaveRequestService) : base(logger)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost(ApiEndPointConstant.LeaveRequestEndpoint.Submit)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitAbsenceRequest([FromBody] LeaveRequestDto leaveRequest)
        {
            Result<bool> result = await _leaveRequestService.SubmitLeaveRequest(leaveRequest);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(ApiEndPointConstant.LeaveRequestEndpoint.AbsenceProcess)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessAbsentRequest([FromBody] AbsenceApproveRequest absenceRequest)
        {
            Result<bool> result = await _leaveRequestService.ProcessLeaveRequest(absenceRequest);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet(ApiEndPointConstant.LeaveRequestEndpoint.Endpoint)]
        [ProducesResponseType(typeof(Result<List<GetLeaveRequest>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] RequestStatus status = RequestStatus.Pending, [FromQuery] Guid? cId = null)
        {
            Result<List<GetLeaveResponse>> result = await _leaveRequestService.GetAll(status, cId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
