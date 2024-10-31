using Xunit;
using NSubstitute;
using Users.Application.Managers.Interfaces;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Services;
using Users.Domain.DTOs.Requests;
using Users.Domain.Entities;
using Users.Domain.DTOs.Responses;
using Users.Domain.Result;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
    public class UserService_LoginAsyncTests
    {
        private readonly IRepositoryManager _mockRepositoryManager;
        private readonly IPasswordManager _mockPasswordManager;
        private readonly ITokenManager _mockTokenManager;
        private readonly IEmailVerificationSender _mockEmailVerificationSender;
        private readonly IFactoryManager _mockFactoryManager;

        private readonly UserService _userService;

        private readonly LoginReqDTO _loginReqDTO;
		private readonly User _testUser;
        private readonly TokenDTO _tokenDTO;

		public UserService_LoginAsyncTests()
        {
            _mockRepositoryManager = Substitute.For<IRepositoryManager>();
            _mockPasswordManager = Substitute.For<IPasswordManager>();
            _mockTokenManager = Substitute.For<ITokenManager>();
            _mockEmailVerificationSender = Substitute.For<IEmailVerificationSender>();
            _mockFactoryManager = Substitute.For<IFactoryManager>();

            _userService = new UserService(
                _mockPasswordManager,
                _mockTokenManager,
                _mockRepositoryManager,
                _mockEmailVerificationSender,
                _mockFactoryManager
            );

			_loginReqDTO = new LoginReqDTO { Email = "test@example.com", Password = "ValidPassword123" };

			_testUser = new User("21212", "test@example.com", "hashedPassword", "salt", "UserRole", "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);

			_tokenDTO = new TokenDTO("someToken");
		}

        [Fact]
        public async Task LoginAsync_ShouldReturnSuccess_WhenLoginIsSuccessful()
        {
            // Arrange
            _mockRepositoryManager.User.GetUserByEmailAsync(_loginReqDTO.Email)
                .Returns(Result<User>.Success(_testUser));

            _mockPasswordManager.VerifyPassword(_loginReqDTO.Password, _testUser.PasswordHash, _testUser.Salt)
                .Returns(true);

            _mockTokenManager.CreateToken(_testUser.Id)
                .Returns(_tokenDTO);

            // Act
            var result = await _userService.LoginAsync(_loginReqDTO);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenPasswordIsIncorrect()
        {
            // Arrange
            _mockRepositoryManager.User.GetUserByEmailAsync(_loginReqDTO.Email)
                .Returns(Result<User>.Success(_testUser));

            _mockPasswordManager.VerifyPassword(_loginReqDTO.Password, _testUser.PasswordHash, _testUser.Salt)
                .Returns(false);

            // Act
            var result = await _userService.LoginAsync(_loginReqDTO);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Response.Should().BeEquivalentTo(Response.IncorrectPassword);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            _mockRepositoryManager.User.GetUserByEmailAsync(_loginReqDTO.Email)
                .Returns(Result<User>.Failure(Response.UserNotFound));

            // Act
            var result = await _userService.LoginAsync(_loginReqDTO);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Response.Should().BeEquivalentTo(Response.UserNotFound);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            _mockRepositoryManager.User.GetUserByEmailAsync(_loginReqDTO.Email)
                .Throws(new Exception("Database error"));

            // Act
            var result = await _userService.LoginAsync(_loginReqDTO);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Response.Should().BeEquivalentTo(Response.InternalError);
        }
    }
}
