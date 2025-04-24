using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Email.Commands;

public class EmailCommandHandlerUnitTests : BaseUsersUnitTest
{
	private readonly HandleEmailCommandHandler _commandHandler;

	public EmailCommandHandlerUnitTests()
	{
		_commandHandler = new HandleEmailCommandHandler(UnitOfWork, EmailVerificationTokenRepository, UserRepository, DateTimeProvider);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenTokenDoesntExist()
	{
		// Arrange
		var token = GetToken();
		var command = new HandleEmailCommand(UsersTestUtilities.InvalidId);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.InvalidVerificationToken);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenTokenHasExpired()
	{
		// Arrange
		var token = GetToken();
		var command = new HandleEmailCommand(token.Id);

		DateTimeProvider.UtcNow.Returns(UsersTestUtilities.FutureDate);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.ExpiredVerificationToken);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyVerified()
	{
		// Arrange
		var token = GetToken(true);
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.EmailAlreadyVerified);
	}

	[Fact]
	public async Task Handle_ShouldCallGetByIdAsync_WhenCommandIsValid()
	{
		// Arrange
		var token = GetToken();
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await EmailVerificationTokenRepository.Received(1).GetByIdAsync(token.Id);
	}

	[Fact]
	public async Task Handle_ShouldVerifyEmail_WhenCommandIsValid()
	{
		// Arrange
		var token = GetToken();
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		token.User.EmailVerified.Should().BeTrue();
	}

	[Fact]
	public async Task Handle_ShouldCallUpdateForUser_WhenCommandIsValid()
	{
		// Arrange
		var token = GetToken();
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		UserRepository.Received(1).Update(token.User);
	}

	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenCommandIsValid()
	{
		// Arrange
		var token = GetToken();
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}
}
