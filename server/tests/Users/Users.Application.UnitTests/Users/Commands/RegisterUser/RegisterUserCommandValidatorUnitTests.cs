using FluentValidation.TestHelper;
using Shared.Domain.Enums;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.RegisterUser;

public class RegisterUserCommandValidatorUnitTests : BaseUsersUnitTest
{
	private readonly RegisterUserCommandValidator _validator;

	public RegisterUserCommandValidatorUnitTests()
	{
		_validator = new RegisterUserCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForEmail_WhenEmailIsNull()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Email);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.EMAIL_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.EMAIL_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForEmail_WhenEmailLengthIsInvalid(int length)
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Email);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForPassword_WhenPasswordIsNull()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Password);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForPassword_WhenPasswordLengthIsInvalid(int length)
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Password);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForFirstName_WhenFirstNameIsNull()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.FirstName);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForFirstName_WhenFirstNameLengthIsInvalid(int length)
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.FirstName);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForLastName_WhenLastNameIsNull()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.LastName);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForLastName_WhenLastNameLengthIsInvalid(int length)
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.LastName);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForDateOfBirth_WhenDateOfBirthIsntInThePast()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.DateOfBirth);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForPhoneNumber_WhenPhoneNumberIsNull()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PhoneNumber);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForPhoneNumber_WhenPhoneNumberLengthIsInvalid(int length)
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PhoneNumber);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForPhoneNumber_WhenPhoneNumberIsInvalid()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PhoneNumber);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForAddress_WhenAddressIsNull()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Address);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.ADRESS_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ADRESS_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForAddress_WhenAddressLengthIsInvalid(int length)
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Address);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForRole_WhenRoleIsInvalid()
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
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Role);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValid()
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
			Roles.Patient
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
