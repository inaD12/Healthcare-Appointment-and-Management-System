using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.UpdateUser;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Commands.UserCommandHandlerTests;

public class UpdateUserCommandHandlerTests : BaseUsersUnitTest
{
	private readonly UpdateUserCommandHandler _commandHandler;

	public UpdateUserCommandHandlerTests()
	{
		_commandHandler = new UpdateUserCommandHandler(RepositoryManager, UnitOfWork);
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenUpdateIsSuccessful()
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.UpdateSuccessful);

		RepositoryManager.User.Received(1).UpdateAsync(
		Arg.Is<User>(user =>
			user.Id == command.Id &&
			user.Email == command.NewEmail &&
			user.FirstName == command.FirstName &&
			user.LastName == command.LastName
			)
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.InvalidId,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);

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
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.TakenEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
			);
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
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			null,
			null,
			UsersTestUtilities.InvalidLastName
			);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.UpdateSuccessful);

		RepositoryManager.User.Received(1).UpdateAsync(
		Arg.Is<User>(user =>
			user.Id == command.Id &&
			user.Email != command.NewEmail &&
			user.FirstName != command.FirstName &&
			user.LastName == command.LastName
			)
		);
	}
}
