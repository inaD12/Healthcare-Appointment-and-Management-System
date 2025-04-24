using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.DeleteUser;

public class DeleteUserCommandHandlerUnitTests : BaseUsersUnitTest
{
	private readonly DeleteUserCommandHandler _commandHandler;

	public DeleteUserCommandHandlerUnitTests()
	{
		_commandHandler = new DeleteUserCommandHandler(UnitOfWork, UserRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
	{
		// Arrange
		var user = GetUser();
		var command = new DeleteUserCommand(UsersTestUtilities.InvalidId);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Handle_ShouldCallDelete_WhenUserExists()
	{
		// Arrange
		var user = GetUser();
		var command = new DeleteUserCommand(user.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		UserRepository.Received(1).Delete(user);
	}

	[Fact]
	public async Task Handle_CallSaveChanges_WhenUserExists()
	{
		// Arrange
		var user = GetUser();
		var command = new DeleteUserCommand(user.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}
}
