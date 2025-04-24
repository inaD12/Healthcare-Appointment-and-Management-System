using FluentAssertions;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Users.Commands.DeleteUser;

public class DeleteUserCommandHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public DeleteUserCommandHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenIdIsNull()
	{
		// Arrange
		var command = new DeleteUserCommand(
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
	[InlineData(UsersBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new DeleteUserCommand(
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
	public async Task Send_ShouldReturnFailure_WhenUserDoesNotExist()
	{
		// Arrange
		var command = new DeleteUserCommand(
			UsersTestUtilities.InvalidId
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Send_ShouldDeleteUser_WhenUserExists()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new DeleteUserCommand(
			user.Id
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);
		var deletedUser = await UserRepository.GetByIdAsync(user.Id);

		// Assert
		result.IsSuccess.Should().BeTrue();
		deletedUser.Should().BeNull();
	}
}
