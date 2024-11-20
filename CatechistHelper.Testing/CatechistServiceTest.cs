using CatechistHelper.Application.GoogleServices;
using CatechistHelper.Domain.Dtos.Requests.Catechist;
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
    public class CatechistServiceTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private readonly CatechistService _catechistService;
        private readonly ApplicationDbContext _testDbContext;

        public CatechistServiceTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testDbContext = new ApplicationDbContext(_contextOptions);

            // Create the AccountService with the in-memory database context
            _catechistService = new CatechistService(Mock.Of<IFirebaseService>(), new UnitOfWork<ApplicationDbContext>(_testDbContext), 
                Mock.Of<ILogger<CatechistService>>(), Mock.Of<IMapper>(), Mock.Of<IHttpContextAccessor>());

            // Seed the database with users
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test catechists to the in-memory database
            var catechist1 = new Catechist
            {
                Id = Guid.NewGuid(),
                Code = "CH001",
                FullName = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                BirthPlace = "New York",
                FatherName = "Mr. Doe",
                MotherName = "Mrs. Doe",
                Phone = "1234567890",
                Address = "123 Main St",
                Qualification = "Bachelor's",
                IsTeaching = true,
                AccountId = Guid.NewGuid(),
                ChristianNameId = Guid.NewGuid(),
                LevelId = Guid.NewGuid()
            };

            var catechist2 = new Catechist
            {
                Id = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96"),
                Code = "CH002",
                FullName = "Jane Doe",
                Gender = "Female",
                DateOfBirth = new DateTime(1992, 2, 2),
                BirthPlace = "Los Angeles",
                FatherName = "Mr. Smith",
                MotherName = "Mrs. Smith",
                Phone = "0987654321",
                Address = "456 Elm St",
                Qualification = "Master's",
                IsTeaching = true,
                AccountId = Guid.NewGuid(),
                ChristianNameId = Guid.NewGuid(),
                LevelId = Guid.NewGuid()
            };

            var account = new Account
            {
                Id = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                Email = "abc@gmail.com",
                HashedPassword = PasswordUtil.HashPassword("123456")
            };

            var level = new Level
            {
                Id = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                Description = "Level Description"
            };

            var christianName = new ChristianName
            {
                Id = new Guid("8199c6f5-8930-45a0-b812-49bec5dfe326"),
                Name = "Abc"
            };

            _testDbContext.Catechists.AddRange(catechist1, catechist2);
            _testDbContext.Accounts.Add(account);
            _testDbContext.Levels.Add(level);
            _testDbContext.ChristianNames.Add(christianName);
            _testDbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllCatechists_ReturnsListOfCatechists()
        {
            // Act
            var result = await _catechistService.GetAll(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Total);
        }

        [Fact]
        public async Task GetCatechistById_ReturnsCatechist_WhenFound()
        {
            // Arrange
            var catechistId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");

            // Act
            var result = await _catechistService.GetById(catechistId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCatechistById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var catechistId = Guid.NewGuid();

            var result = await _catechistService.GetById(catechistId);
            // Act
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCatechist_AddsCatechist()
        {
            // Arrange
            var catechist = new CreateCatechistRequest
            {
                FullName = "John Doe",
                Gender = "Male",
                AccountId = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                LevelId = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                ChristianNameId = new Guid("8199c6f5-8930-45a0-b812-49bec5dfe326")

            };


            // Act
            var result = await _catechistService.CreateAsync(catechist);

            // Assert

            Assert.NotNull(result);

        }

        [Fact]
        public async Task UpdateCatechist_UpdatesCatechist_WhenExists()
        {
            _testDbContext.ChangeTracker.Clear();

            // Arrange
            var catechistId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            var catechist = new UpdateCatechistRequest { 
                FullName = "New Catechist",
                Gender = "Male",
                AccountId = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                LevelId = new Guid("55549961-bb82-4d04-923d-9b631c0b2e4f"),
                ChristianNameId = new Guid("8199c6f5-8930-45a0-b812-49bec5dfe326")
            };

            var result = await _catechistService.Update(catechistId, catechist);

            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteCatechist_DeletesCatechist_WhenExists()
        {

            _testDbContext.ChangeTracker.Clear();
            // Arrange
            var catechistId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");

            var result = await _catechistService.Delete(catechistId);

            Assert.True(result.Data);
        }

    }
}
