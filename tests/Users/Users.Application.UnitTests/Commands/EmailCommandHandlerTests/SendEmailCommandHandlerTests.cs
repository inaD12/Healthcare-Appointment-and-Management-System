using FluentAssertions;
using NSubstitute;
using Shared.Domain.Results;
using Users.Application.Commands.Email.SendEmail;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Commands.EmailCommandHandlerTests;


public class SendEmailCommandHandlerTests : BaseUsersUnitTest
{
	private readonly SendEmailCommandHandler _handler;

	public SendEmailCommandHandlerTests()
	{
		_handler = new SendEmailCommandHandler(FluentEmail, FactoryManager, RepositoryManager);
	}

	[Fact]
	public async Task SendEmailAsync_ShouldSendEmailSuccessfully()
	{
		//Arrange
		var command = new SendEmailCommand(
			UsersTestUtilities.TakenId,
			UsersTestUtilities.ValidEmail
			);

		// Act
		var result = await _handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().BeEquivalentTo(Result.Success());

		await FluentEmail.Received(1).SendAsync();
		await RepositoryManager.EmailVerificationToken.Received(1).AddAsync(
			Arg.Is<EmailVerificationToken>(token =>
				token.User.Email == UsersTestUtilities.TakenEmail)
			);
	}

	[Fact]
	public async Task SendEmailAsync_ShouldReturnFailure_WhenEmailSendingFails()
	{
		// Arrange
		var command = new SendEmailCommand(
			UsersTestUtilities.TakenId,
			UsersTestUtilities.EmailSendingErrorEmail
			);

		// Act
		var result = await _handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().BeEquivalentTo(Result.Failure(Responses.EmailNotSent));

		await FluentEmail.Received(1).SendAsync();
		await RepositoryManager.EmailVerificationToken.DidNotReceive().AddAsync(
			Arg.Is<EmailVerificationToken>(token =>
				token.User.Email == UsersTestUtilities.TakenEmail)
			);
	}
}

