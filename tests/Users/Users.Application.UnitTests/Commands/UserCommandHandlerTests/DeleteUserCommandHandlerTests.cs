using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Commands.UserCommandHandlerTests;

public class DeleteUserCommandHandlerTests : BaseUsersUnitTest
{
	private readonly DeleteUserCommandHandler _commandHandler;

	public DeleteUserCommandHandlerTests()
	{
		_commandHandler = new DeleteUserCommandHandler(UnitOfWork, UserRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenUserExists()
	{
		// Arrange
		var command = new DeleteUserCommand(UsersTestUtilities.ValidId);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UserRepository.Received(1).DeleteByIdAsync(UsersTestUtilities.ValidId);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
	{
		// Arrange
		var command = new DeleteUserCommand(UsersTestUtilities.InvalidId);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.UserNotFound);
		await UserRepository.DidNotReceive().DeleteByIdAsync(Arg.Any<string>());
	}
}
