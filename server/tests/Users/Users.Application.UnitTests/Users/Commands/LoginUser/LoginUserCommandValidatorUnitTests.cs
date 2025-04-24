using FluentValidation.TestHelper;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.Features.Users.LoginUser;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.LoginUser;

public class LoginUserCommandValidatorUnitTests : BaseUsersUnitTest
{
	private readonly LoginUserCommandValidator _validator;

	public LoginUserCommandValidatorUnitTests()
	{
		_validator = new LoginUserCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForEmail_WhenEmailIsNull()
	{
		// Arrange
		var command = new LoginUserCommand(
			null!,
			UsersTestUtilities.ValidPassword
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Email);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(UsersBusinessConfiguration.EMAIL_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.EMAIL_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForEmail_WhenEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new LoginUserCommand(
			SharedTestUtilities.GetString(length),
			UsersTestUtilities.ValidPassword
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
		var command = new LoginUserCommand(
			UsersTestUtilities.ValidEmail,
			null!
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Password);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForPassword_WhenPasswordLengthIsInvalid(int length)
	{
		// Arrange
		var command = new LoginUserCommand(
			UsersTestUtilities.ValidEmail,
			SharedTestUtilities.GetString(length)
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Password);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValid()
	{
		// Arrange
		var command = new LoginUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
