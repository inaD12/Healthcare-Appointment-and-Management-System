using Contracts.Enums;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Application.Services;
using Users.Domain.DTOs.Requests;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Infrastructure.MessageBroker;
using Xunit;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class UserService_UpdateUserAsyncTests
	{
		private readonly UserService _userService;
		private readonly IRepositoryManager _mockRepositoryManager = Substitute.For<IRepositoryManager>();
		private readonly IPasswordManager _mockPasswordManager = Substitute.For<IPasswordManager>();
		private readonly ITokenManager _mockTokenManager = Substitute.For<ITokenManager>();
		private readonly IEmailVerificationSender _mockEmailVerificationSender = Substitute.For<IEmailVerificationSender>();
		private readonly IFactoryManager _mockFactoryManager = Substitute.For<IFactoryManager>();
		private readonly IEventBus _mockEvenyBus = Substitute.For<IEventBus>();

		private readonly string _id = "1";
		private readonly User _testUser;
		private readonly UpdateUserReqDTO _updateUserDTO;

		public UserService_UpdateUserAsyncTests()
		{
			_userService = new UserService(_mockPasswordManager, _mockTokenManager, _mockRepositoryManager, _mockEmailVerificationSender, _mockFactoryManager, _mockEvenyBus);

			_testUser = new User("123", "test@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);

			_updateUserDTO = new UpdateUserReqDTO
			{
				FirstName = "NewFirstName",
				LastName = "NewLastName",
				NewEmail = "newemail@example.com"
			};
		}

		[Fact]
		public async Task UpdateUserAsync_ShouldReturnSuccess_WhenUpdateIsSuccessful()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Success(_testUser));

			_mockRepositoryManager.User.GetUserByEmailAsync(_updateUserDTO.NewEmail)
			.Returns(Result<User>.Failure(Responses.UserNotFound));

			// Act
			var result = await _userService.UpdateUserAsync(_updateUserDTO, _id);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UpdateSuccessful);
		}

		[Fact]
		public async Task UpdateUserAsync_ShouldReturnFailure_WhenUserDoesNotExist()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			// Act
			var result = await _userService.UpdateUserAsync(_updateUserDTO, _id);

			// Assert
			result.IsSuccess.Should().BeFalse();
			result.Response.Should().BeEquivalentTo(Responses.UserNotFound);
		}

		[Fact]
		public async Task UpdateUserAsync_ShouldReturnFailure_WhenNewEmailIsTaken()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Success(_testUser));

			_mockRepositoryManager.User.GetUserByEmailAsync(_updateUserDTO.NewEmail)
			.Returns(Result<User>.Success(_testUser));

			// Act
			var result = await _userService.UpdateUserAsync(_updateUserDTO, _id);

			// Assert
			result.IsSuccess.Should().BeFalse();
			result.Response.Should().BeEquivalentTo(Responses.EmailTaken);
		}

		[Fact]
		public async Task UpdateUserAsync_ShouldUpdateOnlyNonNullFields()
		{
			// Arrange
			var updateDTO = new UpdateUserReqDTO { LastName = "UpdatedLastName" };
			var existingUser = new User(_id, "user@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);

			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Success(existingUser));

			_mockRepositoryManager.User.GetUserByEmailAsync(updateDTO.NewEmail)
			.Returns(Result<User>.Failure(Responses.UserNotFound));

			// Act
			var result = await _userService.UpdateUserAsync(updateDTO, _id);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UpdateSuccessful);
			existingUser.FirstName.Should().Be("John"); // Should remain unchanged
			existingUser.LastName.Should().Be(updateDTO.LastName); // Updated
			existingUser.Email.Should().Be("user@example.com"); // Should remain unchanged
		}
	}
}
