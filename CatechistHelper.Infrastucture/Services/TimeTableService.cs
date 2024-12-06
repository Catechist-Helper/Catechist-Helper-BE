﻿using CatechistHelper.Application.Repositories;
using CatechistHelper.Application.Services;
using CatechistHelper.Domain.Common;
using CatechistHelper.Domain.Dtos.Requests.Timetable;
using CatechistHelper.Domain.Dtos.Responses.Timetable;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Utils;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CatechistHelper.Infrastructure.Services
{
    public class TimeTableService : BaseService<TimeTableService>, ITimetableService
    {
        private readonly IPastoralYearService _pastoralYearService;
        private readonly ISystemConfigurationService _systemConfiguration;

        public TimeTableService(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<TimeTableService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPastoralYearService pastoralYearService,
            ISystemConfigurationService systemConfiguration
        ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _pastoralYearService = pastoralYearService;
            _systemConfiguration = systemConfiguration;
        }

        public async Task<Result<List<SlotResponse>>> CreateSlots(CreateSlotsRequest request)
        {
            try
            {
                var result = await CreateSlot(request);
                return Success(result.Adapt<List<SlotResponse>>());
            }
            catch (Exception ex)
            {
                return Fail<List<SlotResponse>>(ex.Message);
            }
        }

        public async Task<Result<ClassResponse>> CreateTimeTable(CreateTimetableRequest request)
        {
            try
            {
                var result = await CreateClasses(request.File);
                return Success(result.Adapt<ClassResponse>());
            }
            catch (Exception ex)
            {
                return Fail<ClassResponse>(ex.Message);
            }
        }

        public async Task<List<Class>> CreateClasses(IFormFile file)
        {
            var pastoralYearDto = FileHelper.ReadFile(file);

            var pastoralYearFromDb = await _pastoralYearService.GetByName(pastoralYearDto.Name);

            Validator.EnsureNonExist(pastoralYearFromDb);

            var pastoralYear = await _pastoralYearService.Create(pastoralYearDto);

            string[] years = pastoralYear.Name.Split('-');
            var startDate = await GetStartDate(years[0]);
            var endDate = await GetEndDate(years[1]);

            var createdClasses = new List<Class>();

            foreach (var majorDto in pastoralYearDto.Majors)
            {
                var major = await _unitOfWork.GetRepository<Major>()
                    .SingleOrDefaultAsync(predicate: m => m.Name == majorDto.Name);

                Validator.EnsureNonNull(major);

                foreach (var gradeDto in majorDto.Grades)
                {
                    var grade = await CreateGrade(gradeDto, major.Id);

                    foreach (var classDto in gradeDto.Classes)
                    {
                        classDto.StartDate = startDate;
                        classDto.EndDate = endDate;
                        var classEntity = await CreateClass(classDto, grade.Id, pastoralYear.Id);

                        createdClasses.Add(classEntity);
                    }
                }
            }

            await _unitOfWork.CommitAsync();

            return createdClasses;

        }

        private async Task<Grade> CreateGrade(GradeDto gradeDto, Guid majorId)
        {
            var grade = await _unitOfWork.GetRepository<Grade>()
                .SingleOrDefaultAsync(predicate: g => g.Name == gradeDto.Name);

            if (grade == null)
            {
                grade = new Grade
                {
                    Name = gradeDto.Name,
                    MajorId = majorId,
                };

                await _unitOfWork.GetRepository<Grade>().InsertAsync(grade);
            }

            return grade;
        }

        private async Task<Class> CreateClass(ClassDto classDto, Guid gradeId, Guid pastoralYearId)
        {
            var classEntity = new Class
            {
                Name = classDto.Name,
                StartDate = classDto.StartDate,
                EndDate = classDto.EndDate,
                NumberOfCatechist = classDto.NumberOfCatechist,
                GradeId = gradeId,
                PastoralYearId = pastoralYearId
            };

            await _unitOfWork.GetRepository<Class>().InsertAsync(classEntity);
            return classEntity;
        }

        public async Task<List<Slot>> CreateSlot(CreateSlotsRequest request)
        {
            var classDto = await GetClassById(request.ClassId);

            AddCatechistsToClass(request.Catechists, ref classDto);

            var fixedDay = await GetWeekDay();
            var holidayDates = await GetHolidayDates(classDto.StartDate, classDto.EndDate);

            var slots = await CreateSlotsForClass(classDto, request, fixedDay, holidayDates);

            await _unitOfWork.CommitAsync();

            return slots;
        }

        // Add Catechists to the Class
        private void AddCatechistsToClass(List<CatechistSlot> catechists, ref Class classDto)
        {
            foreach (var catechistSlot in catechists)
            {
                var catechistInClass = new CatechistInClass
                {
                    ClassId = classDto.Id,
                    CatechistId = catechistSlot.CatechistId,
                    IsMain = catechistSlot.IsMain
                };

                classDto.CatechistInClasses.Add(catechistInClass);
            }
        }

        // Get list of holidays as DateTime
        public static async Task<List<DateTime>> GetHolidayDates(DateTime startDate, DateTime? endDate)
        {
            var holidays = await HolidayService.GetHolidaysInRange(startDate, endDate);
            return holidays.Select(h => HolidayService.ParseDate(h.Start.Date, "yyyy-MM-dd")).ToList();
        }

        // Create Slots for the Class
        private async Task<List<Slot>> CreateSlotsForClass(Class classDto, CreateSlotsRequest request, DayOfWeek fixedDay, List<DateTime> holidayDates)
        {
            var slots = new List<Slot>();
            var currentDate = AdjustToFixedWeekDay(classDto.StartDate, fixedDay);

            var startTime = await GetStartTime();
            var endTime = await GetEndTime();

            while (currentDate <= classDto.EndDate)
            {
                if (!holidayDates.Contains(currentDate))
                {
                    var slot = CreateSlotInstance(classDto, request, currentDate, startTime, endTime);
                    AddCatechistsToSlot(slot, classDto.CatechistInClasses);
                    await _unitOfWork.GetRepository<Slot>().InsertAsync(slot);
                    slots.Add(slot);
                }

                currentDate = currentDate.AddDays(7); // Move to the next week
            }

            return slots;
        }

        // Adjust start date to the fixed day of the week
        private static DateTime AdjustToFixedWeekDay(DateTime startDate, DayOfWeek fixedDay)
        {
            while (startDate.DayOfWeek != fixedDay)
            {
                startDate = startDate.AddDays(1);
            }
            return startDate;
        }

        // Create a new Slot instance
        private static Slot CreateSlotInstance(Class classDto, CreateSlotsRequest request, DateTime currentDate, DateTime startTime, DateTime endTime)
        {
            return new Slot
            {
                ClassId = classDto.Id,
                RoomId = request.RoomId,
                Date = currentDate,
                StartTime = startTime,
                EndTime = endTime
            };
        }

        // Add Catechists to the Slot
        private static void AddCatechistsToSlot(Slot slot, ICollection<CatechistInClass> catechistInClasses)
        {
            foreach (var catechistInClass in catechistInClasses)
            {
                var catechistInSlot = new CatechistInSlot
                {
                    SlotId = slot.Id,
                    CatechistId = catechistInClass.CatechistId,
                    Type = catechistInClass.IsMain ? CatechistInSlotType.Main.ToString() : CatechistInSlotType.Assistant.ToString()
                };

                slot.CatechistInSlots.Add(catechistInSlot);
            }
        }

        public async Task<DateTime> GetStartTime()
        {
            var config = await _systemConfiguration.GetConfigByKey("starttime");

            return ConvertTime(config.Value);
        }

        public async Task<DateTime> GetEndTime()
        {
            var config = await _systemConfiguration.GetConfigByKey("endtime");

            return ConvertTime(config.Value);
        }

        public static DateTime ConvertTime(string timeString)
        {
            return DateTime.ParseExact(timeString, "HH:mm", null);
        }

        public async Task<DayOfWeek> GetWeekDay()
        {
            var config = await _systemConfiguration.GetConfigByKey("weekday");

            string value = config.Value.Trim();

            if (Enum.TryParse(value, true, out DayOfWeek weekDay))
            {
                return weekDay;
            }
            throw new Exception($"Invalid weekday configuration value: {value}");
        }

        public async Task<DateTime> GetStartDate(string year)
        {
            var startDate = await _systemConfiguration.GetConfigByKey("startdate");
            return ConvertToDateTime(startDate.Value + "/" + year);
        }

        public async Task<DateTime> GetEndDate(string year)
        {
            var endDate = await _systemConfiguration.GetConfigByKey("enddate");
            return ConvertToDateTime(endDate.Value + "/" + year);
        }

        public static DateTime ConvertToDateTime(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public async Task<Class> GetClassById(Guid id)
        {
            return await _unitOfWork.GetRepository<Class>()
                .SingleOrDefaultAsync(predicate: c => c.Id == id);
        }

        public async Task<Class> GetClassByIdIncludeSlots(Guid id)
        {
            return await _unitOfWork.GetRepository<Class>()
                .SingleOrDefaultAsync(
                    predicate: c => c.Id == id,
                    include: q => q.Include(c => c.Slots)
                                    .ThenInclude(s => s.Room)
                                   .Include(c => c.CatechistInClasses)
                                    .ThenInclude(s => s.Catechist)
                );
        }

        public async Task<byte[]> ExportSlotsToExcel(Guid classId)
        {
            var classDto = await GetClassByIdIncludeSlots(classId);
            return FileHelper.ExportToExcel(classDto);
        }

        public async Task<byte[]> ExportPastoralYearToExcel(string pastoralYear)
        {
            var year = await _unitOfWork.GetRepository<PastoralYear>()
                                .SingleOrDefaultAsync(
                                    predicate: y => y.Name == pastoralYear,
                                    include: y => y.Include(c => c.Classes)
                                                     .ThenInclude(s => s.Slots)
                                                        .ThenInclude(r => r.Room)
                                .Include(c => c.Classes)
                                   .ThenInclude(s => s.Slots)
                                   .ThenInclude(s => s.CatechistInSlots)
                                   .ThenInclude(s => s.Catechist)
                                .Include(g => g.Classes)
                                   .ThenInclude(g => g.Grade)
                                );

            return FileHelper.ExportPastoralYearToExcel(year);
        }

        public async Task<byte[]> ExportCatechists()
        {
            var catechist = await _unitOfWork.GetRepository<Catechist>()
                                     .GetListAsync(include: c => c.Include(c => c.Account)
                                                                  .Include(c => c.Level));
            return FileHelper.ExportCatechist(catechist);
        }

    }
}
