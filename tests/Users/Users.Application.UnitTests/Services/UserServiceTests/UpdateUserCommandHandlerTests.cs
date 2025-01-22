using Contracts.Enums;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using Users.Application.Commands.Users.UpdateUser;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Xunit;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class UpdateUserCommandHandlerTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly UpdateUserCommandHandler _commandHandler;

		private readonly string _id = "1";
		private readonly User _testUser;

		public UpdateUserCommandHandlerTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_commandHandler = new UpdateUserCommandHandler(_mockRepositoryManager);

			_testUser = new User(
				_id,
				"test@example.com",
				"hashedPassword",
				"salt",
				Roles.Patient,
				"John",
				"Doe",
				DateTime.UtcNow,
				"1234567890",
				"Address",
				true
			);
		}

		[Fact]
		public async Task Handle_ShouldReturnSuccess_WhenUpdateIsSuccessful()
		{
			// Arrange
			var command = new UpdateUserCommand(_id, "newemail@example.com", "NewFirstName", "NewLastName");

			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Success(_testUser));

			_mockRepositoryManager.User.GetUserByEmailAsync(command.NewEmail)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UpdateSuccessful);
			_testUser.Email.Should().Be(command.NewEmail);
			_testUser.FirstName.Should().Be(command.FirstName);
			_testUser.LastName.Should().Be(command.LastName);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
		{
			// Arrange
			var command = new UpdateUserCommand(_id, null, "NewFirstName", "NewLastName");

			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UserNotFound);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenNewEmailIsTaken()
		{
			// Arrange
			var command = new UpdateUserCommand(_id, "newemail@example.com", "NewFirstName", "NewLastName");

			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Success(_testUser));

			_mockRepositoryManager.User.GetUserByEmailAsync(command.NewEmail)
				.Returns(Result<User>.Success(_testUser));

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.EmailTaken);
		}

		[Fact]
		public async Task Handle_ShouldUpdateOnlyNonNullFields()
		{
			// Arrange
			var command = new UpdateUserCommand(_id, null, null, "UpdatedLastName");

			_mockRepositoryManager.User.GetUserByIdAsync(_id)
				.Returns(Result<User>.Success(_testUser));

			_mockRepositoryManager.User.GetUserByEmailAsync(command.NewEmail)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UpdateSuccessful);
			_testUser.FirstName.Should().Be("John"); // Unchanged
			_testUser.LastName.Should().Be(command.LastName); // Updated
			_testUser.Email.Should().Be("test@example.com"); // Unchanged
		}
	}
}
