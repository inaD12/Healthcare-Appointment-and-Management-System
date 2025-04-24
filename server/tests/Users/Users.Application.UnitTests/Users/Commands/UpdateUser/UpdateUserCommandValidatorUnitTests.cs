using FluentValidation.TestHelper;
using Shared.Domain.Enums;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.UpdateUser;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.UpdateUser;

public class UpdateUserCommandValidatorUnitTests
{
	private readonly UpdateUserCommandValidator _validator;

	public UpdateUserCommandValidatorUnitTests()
	{
		_validator = new UpdateUserCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForId_WheIdIsNull()
	{
		// Arrange
		var command = new UpdateUserCommand(
			null!,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Id);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForId_WhenIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Id);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.EMAIL_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.EMAIL_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForNewEmail_WhenNewEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.NewEmail);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForFirstName_WhenFirstNameLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidLastName
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.FirstName);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForLastName_WhenLastNameLengthIsInvalid(int length)
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			SharedTestUtilities.GetString(length)
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.LastName);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnError_WheEveryFieldIsNull()
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			null!,
			null!,
			null!
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveAnyValidationError();
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValid()
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValidAndOnlyOneFieldIsNotNull()
	{
		// Arrange
		var command = new UpdateUserCommand(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			null!,
			null!
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
