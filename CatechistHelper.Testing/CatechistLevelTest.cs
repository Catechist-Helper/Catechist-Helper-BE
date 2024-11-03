using CatechistHelper.Domain.Dtos.Requests.Level;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Repositories;
using CatechistHelper.Infrastructure.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatechistHelper.Testing
{
    public class CatechistLevelTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private readonly LevelService _levelService;
        private readonly ApplicationDbContext _testDbContext;

        public CatechistLevelTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testDbContext = new ApplicationDbContext(_contextOptions);

            // Create the AccountService with the in-memory database context
            _levelService = new LevelService(new UnitOfWork<ApplicationDbContext>(_testDbContext), Mock.Of<ILogger<LevelService>>(), Mock.Of<IMapper>(), Mock.Of<IHttpContextAccessor>());

            // Seed the database with users
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test users to the in-memory database
            var level = new Level
            {
                HierarchyLevel = 1,
                Description = "Junior Level",
            };
            _testDbContext.Levels.Add(level);

            var level1 = new Level
            {
                HierarchyLevel = 3,
                Description = "Senior Level",
                Id = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96")
            };
            _testDbContext.Levels.Add(level1);

            var level2 = new Level
            {
                HierarchyLevel = 4,
                Description = "Master Level",
            };
            _testDbContext.Levels.Add(level2);


            _testDbContext.SaveChanges();
        }


        [Fact]
        public async Task CreateLevel_ValidData_ShouldCreateNewLevel()
        {
            var request = new CreateLevelRequest
            {
                Description = "Intermediate Level",
                HierarchyLevel = 2
            };

            // Act
            var level = await _levelService.CreateAsync(request);

            // Assert
            Assert.NotNull(level);
            Assert.Equal("Intermediate Level", level.Description);
            Assert.Equal(2, level.HierarchyLevel);
        }

        [Fact]
        public async Task CreateLevel_InvalidData_ShouldThrowValidationException()
        {
            // Arrange
            var request = new CreateLevelRequest
            {
                Description = "", // Invalid  
                HierarchyLevel = -1 // Invalid
            };

            await Assert.ThrowsAsync<Exception>(() => _levelService.CreateAsync(request));
        }

        [Fact]
        public async Task RetrieveLevel_ById_ShouldReturnCorrectLevel()
        {
            var id = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");

            // Act
            var retrievedLevel = await _levelService.GetById(id);

            // Assert
            Assert.NotNull(retrievedLevel);
            Assert.Equal("Senior Level", retrievedLevel.Description);
            Assert.Equal(3, retrievedLevel.HierarchyLevel);
        }

        [Fact]
        public async Task RetrieveAllLevels_ShouldReturnListOfLevels()
        {

            // Act
            var levels = await _levelService.GetAllAsync();

            // Assert
            Assert.NotNull(levels);
            Assert.Equal(3, levels.Size);
        }

        [Fact]
        public async Task DeleteLevel_ShouldRemoveLevelFromDatabase()
        {
            // Arrange
            var id = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");

            var result = await _levelService.Delete(id);

            Assert.NotNull(result);
            Assert.True(result.Data);
        }

    }


}
