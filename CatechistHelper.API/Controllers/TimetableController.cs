using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Dtos.Requests.Timetable;
using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Timetable;
using CatechistHelper.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatechistHelper.API.Controllers
{
    public class TimetableController : BaseController<TimetableController>
    {
        private readonly ITimetableService _timetableService;

        public TimetableController(ILogger<TimetableController> logger, ITimetableService timetableService) : base(logger)
        {
            _timetableService = timetableService;
        }

        [HttpPost(ApiEndPointConstant.Timetable.TimetableEndpoint)]
        [ProducesResponseType(typeof(Result<ClassResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTimetable([FromForm] CreateTimetableRequest request)
        {
            Result<ClassResponse> result = await _timetableService.CreateTimeTable(request);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost(ApiEndPointConstant.Timetable.SlotsEndpoint)]
        [ProducesResponseType(typeof(Result<List<SlotResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSlots([FromBody] CreateSlotsRequest request)
        {
            Result<List<SlotResponse>> result = await _timetableService.CreateSlots(request);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet(ApiEndPointConstant.Timetable.ExportEndpoint)]
        [ProducesResponseType(typeof(Result<IActionResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExportClassSlots([FromRoute] Guid id)
        {
            try
            {
                var result = await _timetableService.ExportSlotsToExcel(id);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClassSlots.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while exporting class slots to Excel for class ID {id}", id);
                return BadRequest(new { message = "An error occurred while exporting the class slots." });
            }
        }
    }
}
