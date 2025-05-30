using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.Commands.UpdateUser;
using Users.Application.Features.Users.UpdateUser;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerUnitTests : BaseUsersUnitTest
{
	private readonly UpdateUserCommandHandler _commandHandler;

	public UpdateUserCommandHandlerUnitTests()
	{
		_commandHandler = new UpdateUserCommandHandler(UnitOfWork, HAMSMapper, UserRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			UsersTestUtilities.InvalidId,
			user.Email,
			user.FirstName,
			user.LastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenNewEmailIsTaken()
	{
		// Arrange
		var user = GetUser();
		var existingUser = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			existingUser.Email,
			null,
			null
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.EmailTaken);
	}

	[Fact]
	public async Task Handle_ShouldCallGetByIdAsync_WhenCommandIsValid()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			user.Email,
			null,
			null
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UserRepository.Received(1).GetByIdAsync(user.Id);
	}

	[Fact]
	public async Task Handle_ShouldUpdateProfile_WhenCommandIsValid()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		user.Email.Should().Be(command.NewEmail);
		user.FirstName.Should().Be(command.FirstName);
		user.LastName.Should().Be(command.LastName);
	}

	[Fact]
	public async Task Handle_ShouldCallUpdate_WhenCommandIsValid()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		UserRepository.Received(1).Update(Arg.Is<User>(u =>
		u.Id == user.Id &&
		u.Email == command.NewEmail &&
		u.FirstName == command.FirstName &&
		u.LastName == command.LastName
		));
	}

	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenCommandIsValid()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnCorrectModel_WhenUpdateIsSuccessful()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value!.Id.Should().Be(command.Id);
	}

	[Fact]
	public async Task Handle_ShouldUpdateOnlyNonNullFields()
	{
		// Arrange
		var user = GetUser();
		var command = new UpdateUserCommand(
			user.Id,
			null,
			null,
			UsersTestUtilities.ValidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		UserRepository.Received(1).Update(
		Arg.Is<User>(user =>
			user.Id == command.Id &&
			user.Email != command.NewEmail &&
			user.FirstName != command.FirstName &&
			user.LastName == command.LastName
			)
		);
	}
}
