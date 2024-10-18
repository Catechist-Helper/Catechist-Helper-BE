using CatechistHelper.Domain.Dtos.Requests.Timetable;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Repositories;
using CatechistHelper.Infrastructure.Services;
using CatechistHelper.Infrastructure.Utils;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;

namespace CatechistHelper.Testing
{
    public class TimetableTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private readonly TimeTableService _classService;
        private readonly ApplicationDbContext _testDbContext;
        private readonly PastoralYearService _pastoralYearService;
        private readonly SystemConfigurationService _systemConfigurationService;

        public TimetableTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testDbContext = new ApplicationDbContext(_contextOptions);

            _pastoralYearService = new PastoralYearService(
                new UnitOfWork<ApplicationDbContext>(_testDbContext),
                Mock.Of<ILogger<PastoralYearService>>(), Mock.Of<IMapper>(),
                Mock.Of<IHttpContextAccessor>());

            _systemConfigurationService = new SystemConfigurationService(
                 new UnitOfWork<ApplicationDbContext>(_testDbContext),
                Mock.Of<ILogger<SystemConfigurationService>>(), Mock.Of<IMapper>(),
                Mock.Of<IHttpContextAccessor>());


            _classService = new TimeTableService(
                new UnitOfWork<ApplicationDbContext>(_testDbContext),
                Mock.Of<ILogger<TimeTableService>>(), Mock.Of<IMapper>(),
                Mock.Of<IHttpContextAccessor>(),
                _pastoralYearService,
                _systemConfigurationService
                );
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test users to the in-memory database
            var major = new Major
            {
                Name = "Chiên Con"
            };
            _testDbContext.Majors.Add(major);

            var major2 = new Major
            {
                Name = "Ấu Nhi"
            };
            _testDbContext.Majors.Add(major2);

            var major3 = new Major
            {
                Name = "Nghĩa Sĩ"
            };
            _testDbContext.Majors.Add(major3);

            var major4 = new Major
            {
                Name = "Thiếu Nhi"
            };
            _testDbContext.Majors.Add(major4);

            var major5 = new Major
            {
                Name = "Hiệp Sĩ"
            };
            _testDbContext.Majors.Add(major5);


            var config = new SystemConfiguration
            {
                Key = "startdate",
                Value = "05/09"
            };

            var config2 = new SystemConfiguration
            {
                Key = "enddate",
                Value = "09/05"
            };

            var config3 = new SystemConfiguration
            {
                Key = "weekday",
                Value = "Sunday"
            };

            _testDbContext.SystemConfigurations.Add(config);
            _testDbContext.SystemConfigurations.Add(config2);
            _testDbContext.SystemConfigurations.Add(config3);

            var startDate = DateTime.ParseExact("05/09/2024", "dd/mm/yyyy", CultureInfo.InvariantCulture);

            var endDate = DateTime.ParseExact("09/05/2025", "dd/mm/yyyy", CultureInfo.InvariantCulture);

            var pastoralYear = new PastoralYear
            {
                Name = "2024-2025"
            };
            _testDbContext.PastoralYears.Add(pastoralYear);

            var grade = new Grade
            {
                Major = major,
                Name = "Tiếp Sức 1",
                PastoralYear = pastoralYear,
            };
            _testDbContext.Grades.Add(grade);

            var class1 = new Class
            {
                Id = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96"),
                Name = "Lớp 1",
                StartDate = startDate,
                EndDate = endDate,
            };
            _testDbContext.Classes.Add(class1);

            var room = new Room
            {
                Id = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                Name = "Phòng học"
            };
            _testDbContext.Rooms.Add(room);

            _testDbContext.SaveChanges();
        }



        [Fact]
        public async Task ImportTimetable_ReturnListClasses()
        {
            var filePath = "C:\\Users\\ADMIN\\Downloads\\class_data.xlsx";

            var file = FileHelper.ReadFileToIFormFile(filePath);

            var result = await _classService.CreateClasses(file);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(7, result.Count);
        }


        [Fact]
        public async Task FetchHoliday_ReturnListHolidays()
        {
            var startDate = DateTime.ParseExact("05/09/2024", "dd/mm/yyyy", CultureInfo.InvariantCulture);

            var endDate = DateTime.ParseExact("09/05/2025", "dd/mm/yyyy", CultureInfo.InvariantCulture);

            var result = await HolidayService.GetHolidaysInRange(startDate, endDate);

            Assert.NotNull(result);
        }


        [Fact]
        public async Task CreateSlotsTest()
        {
            var classId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");

            var roomId = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f");

            var request = new CreateSlotsRequest
            {
                ClassId = classId,
                RoomId = roomId,
            };

            var result = await _classService.CreateSlot(request);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
