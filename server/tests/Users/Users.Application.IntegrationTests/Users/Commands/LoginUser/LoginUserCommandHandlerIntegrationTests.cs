using FluentAssertions;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.LoginUser;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Users.Commands.LoginUser;

public class LoginUserCommandHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public LoginUserCommandHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenEmailIsNull()
	{
		// Arrange
		var command = new LoginUserCommand(
			null!,
			UsersTestUtilities.ValidPassword
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.EMAIL_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.EMAIL_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new LoginUserCommand(
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidPassword
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenPasswordIsNull()
	{
		// Arrange
		var command = new LoginUserCommand(
			UsersTestUtilities.ValidEmail,
			null!
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenPasswordLengthIsInvalid(int length)
	{
		// Arrange
		var command = new LoginUserCommand(
			UsersTestUtilities.ValidEmail,
			SharedTestUtilities.GetString(length)
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenUserNotFound()
	{
		// Arrange
		var command = new LoginUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenPasswordIsIncorrect()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new LoginUserCommand(
			user.Email,
			UsersTestUtilities.InvalidPassword
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.IncorrectPassword);
	}

	[Fact]
	public async Task Send_ShouldReturnValidToken_WhenModelIsCorrect()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new LoginUserCommand(
			user.Email,
			Password
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeNull();
		ClaimsExtractor.GetUserId(result.Value.Token).Should().Be(user.Id);
		ClaimsExtractor.GetUserRole(result.Value.Token).Should().Be(user.Role.ToString());
	}
}
