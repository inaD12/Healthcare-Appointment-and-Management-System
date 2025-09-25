using FluentAssertions;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.UpdateUser;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public UpdateUserCommandHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WheIdIsNull()
	{
		// Arrange
		var command = new UpdateUserCommand(
			null!,
			UsersTestUtilities.ValidUpdateEmail,
			UsersTestUtilities.ValidUpdateFirstName,
			UsersTestUtilities.ValidUpdateLastName
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
		var command = new UpdateUserCommand(
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidUpdateEmail,
			UsersTestUtilities.ValidUpdateFirstName,
			UsersTestUtilities.ValidUpdateLastName
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
	public async Task Send_ShouldThrowValidationException_WhenNewEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidUpdateFirstName,
			UsersTestUtilities.ValidUpdateLastName
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenFirstNameLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidUpdateEmail,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidUpdateLastName
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenLastNameLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidUpdateEmail,
			UsersTestUtilities.ValidUpdateFirstName,
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
	public async Task Send_ShouldThrowValidationException_WheEveryFieldIsNull()
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			null!,
			null!,
			null!
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
		var command = new UpdateUserCommand(
			UsersTestUtilities.InvalidId,
			UsersTestUtilities.ValidUpdateEmail,
			UsersTestUtilities.ValidUpdateFirstName,
			UsersTestUtilities.ValidUpdateLastName
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenNewEmailIsTaken()
	{
		// Arrange
		var user = await CreateUserAsync();
		var takneEmailUser = await CreateUserAsync(UsersTestUtilities.ValidUpdateEmail);
		var command = new UpdateUserCommand(
			user.Id,
			takneEmailUser.Email,
			UsersTestUtilities.ValidUpdateFirstName,
			UsersTestUtilities.ValidUpdateLastName
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.EmailTaken);
	}

	[Fact]
	public async Task Send_ShouldUpdateUser_WhenModelIsCorrect()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new UpdateUserCommand(
			user.Id,
			UsersTestUtilities.ValidUpdateEmail,
			UsersTestUtilities.ValidUpdateFirstName,
			UsersTestUtilities.ValidUpdateLastName
		);

		var updatedUser = await UserRepository.GetByIdAsync(user.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		updatedUser.Should().Match<User>(p => p.Id == user.Id &&
									  p.Email == command.NewEmail &&
									  p.FirstName == command.FirstName &&
									  p.LastName == command.LastName &&
									  p.Role == user.Role &&
									  p.Address == user.Address &&
									  p.PhoneNumber == user.PhoneNumber &&
									  p.DateOfBirth == user.DateOfBirth &&
									  p.IdentityId == user.IdentityId);
	}

	[Fact]
	public async Task Send_ShouldUpdateUser_WhenModelContainsOnlyEmail()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new UpdateUserCommand(
			user.Id,
			UsersTestUtilities.ValidUpdateEmail,
			null!,
			null!
		);

		var updatedUser = await UserRepository.GetByIdAsync(user.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		updatedUser.Should().Match<User>(p => p.Id == user.Id &&
									  p.Email == command.NewEmail &&
									  p.FirstName == user.FirstName &&
									  p.LastName == user.LastName &&
									  p.Role == user.Role &&
									  p.Address == user.Address &&
									  p.PhoneNumber == user.PhoneNumber &&
									  p.DateOfBirth == user.DateOfBirth &&
									  p.IdentityId == user.IdentityId);
	}

	[Fact]
	public async Task Send_ShouldUpdateUser_WhenModelContainsOnlyFirstName()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new UpdateUserCommand(
			user.Id,
			null!,
			UsersTestUtilities.ValidUpdateFirstName,
			null!
		);

		var updatedUser = await UserRepository.GetByIdAsync(user.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		updatedUser.Should().Match<User>(p => p.Id == user.Id &&
									  p.Email == user.Email &&
									  p.FirstName == command.FirstName &&
									  p.LastName == user.LastName &&
									  p.Role == user.Role &&
									  p.Address == user.Address &&
									  p.PhoneNumber == user.PhoneNumber &&
									  p.DateOfBirth == user.DateOfBirth &&
									  p.IdentityId == user.IdentityId);
	}

	[Fact]
	public async Task Send_ShouldUpdateUser_WhenModelContainsOnlyLastName()
	{
		// Arrange
		var user = await CreateUserAsync();
		var command = new UpdateUserCommand(
			user.Id,
			null!,
			null!,
			UsersTestUtilities.ValidUpdateLastName
		);

		var updatedUser = await UserRepository.GetByIdAsync(user.Id);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		updatedUser.Should().Match<User>(p => p.Id == user.Id &&
									  p.Email == user.Email &&
									  p.FirstName == user.FirstName &&
									  p.LastName == command.LastName &&
									  p.Role == user.Role &&
									  p.Address == user.Address &&
									  p.PhoneNumber == user.PhoneNumber &&
									  p.DateOfBirth == user.DateOfBirth &&
									  p.IdentityId == user.IdentityId);
	}
}
