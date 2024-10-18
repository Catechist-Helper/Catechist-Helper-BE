using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using CatechistHelper.Infrastructure.Repositories;
using CatechistHelper.Domain.Dtos.Requests.Major;

namespace CatechistHelper.Testing
{
    public class MajorTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private readonly MajorService _majorService;
        private readonly ApplicationDbContext _testDbContext;

        public MajorTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testDbContext = new ApplicationDbContext(_contextOptions);

            // Create the MajorService with the in-memory database context
            _majorService = new MajorService(new UnitOfWork<ApplicationDbContext>(_testDbContext), Mock.Of<ILogger<MajorService>>(), Mock.Of<IMapper>(), Mock.Of<IHttpContextAccessor>());

            // Seed the database with users
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test majors to the in-memory database
            Major major1 = new Major
            {
                Name = "Xưng tội",
            };
            _testDbContext.Majors.Add(major1);

            Major major2 = new Major
            {
                Name = "Thêm sức"
            };
            _testDbContext.Majors.Add(major2);

            _testDbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllMajors_ReturnsListOfMajors()
        {
            // Act
            var result = await _majorService.GetPagination(1, 10);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
        }
        [Fact]
        public async Task GetMajorById_ReturnsMajor_WhenFound()
        {
            // Arrange
            var catechistId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            // Act
            var result = await _majorService.Get(catechistId);
            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMajorById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var catechistId = Guid.NewGuid();
            var result = await _majorService.Get(catechistId);
            // Act
            Assert.Null(result);
        }
        [Fact]
        public async Task CreateMajor_AddsMajor()
        {
            // Arrange
            CreateMajorRequest majorRequest = new CreateMajorRequest
            {
                Name = "Xưng tội",
            };
            // Act
            var result = await _majorService.Create(majorRequest);
            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateMajor_UpdatesMajor_WhenExists()
        {
            _testDbContext.ChangeTracker.Clear();
            // Arrange
            var majorId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            var major = new UpdateMajorRequest
            {
                Name = "Thêm sức"    
            };
            var result = await _majorService.Update(majorId, major);
            Assert.True(result.Data);
        }
        [Fact]
        public async Task DeleteCatechist_DeletesCatechist_WhenExists()
        {
            _testDbContext.ChangeTracker.Clear();
            // Arrange
            var majorId = new Guid("7b03cc3c-4a6a-4849-9673-1ef9ca8e2f96");
            var result = await _majorService.Delete(majorId);
            Assert.True(result.Data);
        }
    }
}
