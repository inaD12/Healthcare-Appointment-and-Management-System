using FluentValidation.TestHelper;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.DeleteUser;

public class DeleteUserCommandValidatorUnitTests : BaseUsersUnitTest
{
	private readonly DeleteUserCommandValidator _validator;

	public DeleteUserCommandValidatorUnitTests()
	{
		_validator = new DeleteUserCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForId_WhenIdIsNull()
	{
		// Arrange
		var command = new DeleteUserCommand(
			null!
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Id);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(UsersBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForId_WhenIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new DeleteUserCommand(
			SharedTestUtilities.GetString(length)
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Id);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValid()
	{
		// Arrange
		var command = new DeleteUserCommand(
			UsersTestUtilities.ValidId
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
