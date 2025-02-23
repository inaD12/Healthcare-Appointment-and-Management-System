using FluentAssertions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Users.LoginUser;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Commands.UserCommandHandlerTests;

public class LoginCommandHandlerTests : BaseUsersUnitTest
{
	private readonly LoginUserCommandHandler _commandHandler;

	public LoginCommandHandlerTests()
	{
		_commandHandler = new LoginUserCommandHandler(RepositoryManager, PasswordManager, TokenManager);
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenLoginIsSuccessful()
	{
		// Arrange
		var command = new LoginUserCommand<TokenResult>(UsersTestUtilities.TakenEmail, UsersTestUtilities.ValidPassword);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().BeEquivalentTo(UsersTestUtilities.TokenDTO);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenPasswordIsIncorrect()
	{
		// Arrange
		var command = new LoginUserCommand<TokenResult>(UsersTestUtilities.TakenEmail, UsersTestUtilities.InvalidPassword);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.IncorrectPassword);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
	{
		// Arrange
		var command = new LoginUserCommand<TokenResult>(UsersTestUtilities.UnusedEmail, UsersTestUtilities.ValidPassword);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.UserNotFound);
	}
}
