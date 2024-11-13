using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Application.Services;
using Users.Domain.Entities;
using Users.Domain.Result;
using Users.Infrastructure.MessageBroker;
using Xunit;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class UserService_DeleteUserAsyncTests
	{
		private readonly UserService _userService;
		private readonly IRepositoryManager _mockRepositoryManager = Substitute.For<IRepositoryManager>();
		private readonly IPasswordManager _mockPasswordManager = Substitute.For<IPasswordManager>();
		private readonly ITokenManager _mockTokenManager = Substitute.For<ITokenManager>();
		private readonly IEmailVerificationSender _mockEmailVerificationSender = Substitute.For<IEmailVerificationSender>();
		private readonly IFactoryManager _mockFactoryManager = Substitute.For<IFactoryManager>();
		private readonly IEventBus _mockEventBus = Substitute.For<IEventBus>();

		private readonly string _existingUserId = "existingUserId";
		private readonly string _nonExistentUserId = "nonExistentUserId";

		private readonly User _testUser;

		public UserService_DeleteUserAsyncTests()
		{
			_userService = new UserService(_mockPasswordManager, _mockTokenManager, _mockRepositoryManager, _mockEmailVerificationSender, _mockFactoryManager, _mockEventBus);

			_testUser = new User(_existingUserId, "test@example.com", "hashedPassword", "salt", "UserRole", "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);
		}

		[Fact]
		public async Task DeleteUserAsync_ShouldReturnSuccess_WhenUserExists()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_existingUserId)
				.Returns(Result<User>.Success(_testUser));
			_mockRepositoryManager.User.DeleteUserAsync(_existingUserId)
				.Returns(Task.CompletedTask);

			// Act
			var result = await _userService.DeleteUserAsync(_existingUserId);

			// Assert
			result.IsSuccess.Should().BeTrue();
			_mockRepositoryManager.User.Received(1).DeleteUserAsync(_existingUserId);
		}

		[Fact]
		public async Task DeleteUserAsync_ShouldReturnFailure_WhenUserDoesNotExist()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_nonExistentUserId)
				.Returns(Result<User>.Failure(Response.UserNotFound));

			// Act
			var result = await _userService.DeleteUserAsync(_nonExistentUserId);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.UserNotFound);
			_mockRepositoryManager.User.DidNotReceive().DeleteUserAsync(Arg.Any<string>());
		}

		[Fact]
		public async Task DeleteUserAsync_ShouldReturnFailure_WhenExceptionOccurs()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_existingUserId)
				.Returns(Result<User>.Success(_testUser));

			_mockRepositoryManager.User.DeleteUserAsync(_existingUserId)
				.Throws(new Exception("Database error"));

			// Act
			var result = await _userService.DeleteUserAsync(_existingUserId);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.InternalError);
		}

	}
}
