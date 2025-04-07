using FluentAssertions;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Email.Commands;

public class HandleEmailCommandHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public HandleEmailCommandHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenTokenIsNotFound()
	{
		// Arrange
		var command = new HandleEmailCommand(UsersTestUtilities.InvalidId);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.InvalidVerificationToken);
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenTokenIsExpired()
	{
		// Arrange
		var token = await CreateEmailVerificationTokenAsync(UsersTestUtilities.PastDate);
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.ExpiredVerificationToken);
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenEmailAlreadyVerified()
	{
		// Arrange
		var token = await CreateEmailVerificationTokenWithEmailVerifiedUserAsync();
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.EmailAlreadyVerified);
	}

	[Fact]
	public async Task Send_ShouldVerifyEmail_WhenTokenIsValid()
	{
		// Arrange
		var token = await CreateEmailVerificationTokenAsync();
		var command = new HandleEmailCommand(token.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);
		var user = await UserRepository.GetByIdAsync(token.User.Id);

		// Assert
		result.IsSuccess.Should().BeTrue();
		user.Should().NotBeNull();
		user.EmailVerified.Should().BeTrue();
	}
}
