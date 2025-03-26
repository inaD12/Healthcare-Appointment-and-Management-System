using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.LoginUser;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.LoginUser;

public class LoginCommandHandlerUnitTests : BaseUsersUnitTest
{
	private readonly LoginUserCommandHandler _commandHandler;

	public LoginCommandHandlerUnitTests()
	{
		_commandHandler = new LoginUserCommandHandler(PasswordManager, TokenFactory, HAMSMapper, UserRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
	{
		// Arrange
		var user = GetUser(out string password);
		var command = new LoginUserCommand(UsersTestUtilities.InvalidEmail, password);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenPasswordIsIncorrect()
	{
		// Arrange
		var user = GetUser();
		var command = new LoginUserCommand(user.Email, UsersTestUtilities.InvalidPassword);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.IncorrectPassword);
	}

	[Fact]
	public async Task Handle_ShouldCallGetByEmailAsync_WhenModelIsCorrect()
	{
		// Arrange
		var user = GetUser(out string password);
		var command = new LoginUserCommand(user.Email, password);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UserRepository.Received(1).GetByEmailAsync(user.Email);
	}

	[Fact]
	public async Task Handle_ShouldCallVerifyPassword_WhenModelIsCorrect()
	{
		// Arrange
		var user = GetUser(out string password);
		var command = new LoginUserCommand(user.Email, password);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		PasswordManager.Received(1).VerifyPassword(password, user.PasswordHash, user.Salt);
	}

	[Fact]
	public async Task Handle_ShouldCallCreateToken_WhenModelIsCorrect()
	{
		// Arrange
		var user = GetUser(out string password);
		var command = new LoginUserCommand(user.Email, password);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		TokenFactory.Received(1).CreateToken(user.Id, user.Role);
	}
}
