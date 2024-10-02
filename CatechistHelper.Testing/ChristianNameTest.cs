using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using CatechistHelper.Infrastructure.Repositories;
using CatechistHelper.Domain.Dtos.Requests.ChristianName;
using Microsoft.Extensions.Configuration;

namespace CatechistHelper.Testing
{
    public class ChristianNameTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private readonly ChristianNameService  _christianNameService;
        private readonly ApplicationDbContext _testDbContext;

        public ChristianNameTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testDbContext = new ApplicationDbContext(_contextOptions);

            // Create the MajorService with the in-memory database context
            _christianNameService = new ChristianNameService(
                new UnitOfWork<ApplicationDbContext>(_testDbContext), 
                Mock.Of<ILogger<ChristianNameService>>(), 
                Mock.Of<IMapper>(), 
                Mock.Of<IHttpContextAccessor>());

            // Seed the database with users
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test majors to the in-memory database
            ChristianName christianName1 = new ChristianName
            {
                Name = "Alphongsô",
                Gender = "Nam",
                HolyDay = new DateTime(1787, 8, 1)
            };
            _testDbContext.ChristianNames.Add(christianName1);

            ChristianName christianName2 = new ChristianName
            {
                Name = "Agata",
                Gender = "Nữ",
                HolyDay = new DateTime(251, 5, 2)
            };
            _testDbContext.ChristianNames.Add(christianName2);

            _testDbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllChristianNames_ReturnsListOfChristianNames()
        {
            // Act
            var result = await _christianNameService.GetPagination(x => false, 1, 10);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
        }
        [Fact]
        public async Task GetChristianNameById_ReturnsChristianName_WhenFound()
        {
            // Arrange
            var christianNameId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            // Act
            var result = await _christianNameService.Get(christianNameId);
            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetChristianNameById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var christianNameId = Guid.NewGuid();
            var result = await _christianNameService.Get(christianNameId);
            // Act
            Assert.Null(result);
        }
        [Fact]
        public async Task CreateChristianName_AddsChristianName()
        {
            // Arrange
            CreateChristianNameRequest christianNameRequest = new CreateChristianNameRequest
            {
                Name = "Agata",
                Gender = "Nữ",
                HolyDay = new DateTime(251, 5, 2)
            };
            // Act
            var result = await _christianNameService.Create(christianNameRequest);
            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateMajor_UpdatesMajor_WhenExists()
        {
            _testDbContext.ChangeTracker.Clear();
            // Arrange
            var christianNameId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            var christianName = new UpdateChristianNameRequest
            {
                Name = "Thêm sức"
            };
            var result = await _christianNameService.Update(christianNameId, christianName);
            Assert.True(result.Data);
        }
        [Fact]
        public async Task DeleteCatechist_DeletesCatechist_WhenExists()
        {
            _testDbContext.ChangeTracker.Clear();
            // Arrange
            var christianNameId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            var result = await _christianNameService.Delete(christianNameId);
            Assert.True(result.Data);
        }
    }
}
