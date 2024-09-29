using CatechistHelper.Domain.Entities;
using CatechistHelper.Infrastructure.Database;
using CatechistHelper.Infrastructure.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using CatechistHelper.Infrastructure.Utils;
using CatechistHelper.Infrastructure.Repositories;

namespace CatechistHelper.Testing
{
    public class AuthenticationTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private readonly AccountService _accountService;
        private readonly ApplicationDbContext _testDbContext;

        public AuthenticationTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _testDbContext = new ApplicationDbContext(_contextOptions);

            // Create the AccountService with the in-memory database context
            _accountService = new AccountService(new UnitOfWork<ApplicationDbContext>(_testDbContext), Mock.Of<ILogger<AccountService>>(), Mock.Of<IMapper>(), Mock.Of<IHttpContextAccessor>());

            // Seed the database with users
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Add test users to the in-memory database
            var user = new Account
            {
                Email = "test@example.com",
                HashedPassword = PasswordUtil.HashPassword("Password123!")
            };
            _testDbContext.Accounts.Add(user);

            var user2 = new Account
            {
                Email = "test2@example.com",
                HashedPassword = PasswordUtil.HashPassword("Password123!")
            };
            _testDbContext.Accounts.Add(user2);

            _testDbContext.SaveChanges();
        }

        [Fact]
        public async Task ValidateLoginRequest_ValidCredentials_ReturnsUser()
        {
            // Arrange
            string email = "test@example.com";
            string password = "Password123!";

            // Act
            var result = await _accountService.ValidateLoginRequest(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            Assert.NotEmpty(result.HashedPassword);
        }

        [Fact]
        public async Task ValidateLoginRequest_InvalidPassword_ThrowsException()
        {
            // Arrange
            string email = "test@example.com";
            string password = "WrongPassword";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _accountService.ValidateLoginRequest(email, password));
        }

        [Fact]
        public async Task ValidateLoginRequest_UserNotFound_ThrowsException()
        {
            // Arrange
            string email = "nonexistent@example.com";
            string password = "Password123!";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _accountService.ValidateLoginRequest(email, password));
        }
    }
}
