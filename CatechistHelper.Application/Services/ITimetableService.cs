﻿using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Timetable;
using CatechistHelper.Domain.Dtos.Responses.Timetable;

namespace CatechistHelper.Application.Services
{
    public interface ITimetableService
    {
        Task<Result<List<SlotResponse>>> CreateSlots(CreateSlotsRequest request);
        Task<Result<ClassResponse>> CreateTimeTable(CreateTimetableRequest request);
        Task<byte[]> ExportSlotsToExcel(Guid classId);
        Task<byte[]> ExportPastoralYearToExcel(string pastoralYear);
        Task<byte[]> ExportCatechists();
        Task<DateTime> GetStartTime();
        Task<DateTime> GetEndTime();
        Task<DateTime> GetStartDate(string year);
        Task<DateTime> GetEndDate(string year);
    }
}
