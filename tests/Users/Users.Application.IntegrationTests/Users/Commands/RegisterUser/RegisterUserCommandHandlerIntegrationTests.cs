using FluentAssertions;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Enums;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.Models;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Entities;
using Users.Domain.Events;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Users.Commands.RegisterUser;

public class RegisterUserCommandHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public RegisterUserCommandHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenEmailIsNull()
	{
		// Arrange
		var command = new RegisterUserCommand(
			null!,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
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
	public async Task SendAsnyc_ShouldThrowValidationException_WhenEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RegisterUserCommand(
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenPasswordIsNull()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			null!,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH + 1)]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenPasswordLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenFirstNameIsNull()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			null!,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
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
	public async Task SendAsnyc_ShouldThrowValidationException_WhenFirstNameLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenLastNameIsNull()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			null!,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
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
	public async Task SendAsnyc_ShouldThrowValidationException_WhenLastNameLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenDateOfBirthIsntInThePast()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.FutureDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenPhoneNumberIsNull()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			null!,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH + 1)]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenPhoneNumberLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			SharedTestUtilities.GetLong(length).ToString(),
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenPhoneNumberIsInvalid()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.InvalidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenAddressIsNull()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			null!,
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.ADRESS_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ADRESS_MAX_LENGTH + 1)]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenAddressLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			SharedTestUtilities.GetString(length),
			Roles.Patient
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsnyc_ShouldThrowValidationException_WhenRoleIsInvalid()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			(Roles)99
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task SendAsync_ShouldReturnFailure_WhenEmailIsTaken()
	{
		// Arrange
		var existingUser = await CreateUserAsync(); ;
		var command = new RegisterUserCommand(
				existingUser.Email,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Roles.Patient
			);

		// Act
		var res = await Sender.Send(command);

		// Assert
		res.IsFailure.Should().BeTrue();
		res.Response.Should().BeEquivalentTo(ResponseList.EmailTaken);
	}

	[Fact]
	public async Task SendAsync_ShouldRegisterUser_WhenUserIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);
		var user = await UserRepository.GetByEmailAsync(command.Email);

		// Assert
		response.IsSuccess.Should().BeTrue();
		user
			.Should()
			.Match<User>(p => p.Email == command.Email &&
							p.FirstName == command.FirstName &&
							p.LastName == command.LastName &&
							p.PhoneNumber == command.PhoneNumber &&
							p.DateOfBirth == command.DateOfBirth &&
							p.Address == command.Address &&
							p.Role == command.Role &&
							PasswordManager.VerifyPassword(command.Password, p.PasswordHash, p.Salt));
	}

	[Fact]
	public async Task SendAsync_ShouldReturnCorrectViewModel_WhenUserIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);
		var user = await UserRepository.GetByEmailAsync(command.Email);

		// Assert
		user.Should().NotBeNull();
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().Match<UserCommandViewModel>(p => p.Id == user.Id);
	}

	[Fact]
	public async Task SendAsync_ShouldPublishUserCreatedDomainEvent_WhenUserIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();

		await TestHarness.InactivityTask;
		var result = await TestHarness.Published.Any<UserCreatedDomainEvent>(m =>
							  m.Context.Message.Id == response.Value!.Id &&
							  m.Context.Message.Email == command.Email &&
							  m.Context.Message.Role == command.Role,
							  CancellationToken);

		result.Should().BeTrue();
	}

	[Fact]
	public async Task UserCreatedDomainEvent_ShouldBeConsumed_WhenUserIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();

		await TestHarness.InactivityTask;
		var consumedByConsumer = await TestHarness
			.Consumed.Any<UserCreatedDomainEvent>(m =>
				m.Context.Message.Id == response.Value!.Id &&
				m.Context.Message.Email == command.Email &&
				m.Context.Message.Role == command.Role,
				CancellationToken);
	}

	[Fact]
	public async Task UserCreatedIntegrationEvent_ShouldBePublished_WhenUserIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();

		await TestHarness.InactivityTask;
		var result = await TestHarness.Published.Any<UserCreatedIntegrationEvent>(m =>
							  m.Context.Message.Id == response.Value!.Id &&
							  m.Context.Message.Email == command.Email &&
							  m.Context.Message.Role == command.Role,
							  CancellationToken);

		result.Should().BeTrue();
	}

	[Fact]
	public async Task EmailConfirmationRequestedIntegrationEvent_ShouldBePublished_WhenUserIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();

		await TestHarness.InactivityTask;
		var result = await TestHarness.Published.Any<EmailConfirmationRequestedIntegrationEvent>(m =>
							  m.Context.Message.Email == command.Email,
							  CancellationToken);

		result.Should().BeTrue();
	}
}
