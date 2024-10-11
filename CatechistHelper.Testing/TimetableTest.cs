using CatechistHelper.Application.Services;
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


            // Create the AccountService with the in-memory database context
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

            _testDbContext.SystemConfigurations.Add(config);
            _testDbContext.SystemConfigurations.Add(config2);

            _testDbContext.SaveChanges();
        }



        [Fact]
        public async Task ImportTimetable_ReturnListClasses()
        {
            var filePath = "C:\\Users\\ADMIN\\Downloads\\class_data.xlsx"; // Path to a test Excel file

            var file = FileHelper.ReadFileToIFormFile(filePath);

            var result = await _classService.CreateTimetable(file);

            Assert.NotNull(result); // Ensure that the returned list is not null
            Assert.NotEmpty(result); // Ensure that classes were created
            Assert.Equal(7, result.Count);
        }
    }
}
