using FluentValidation.TestHelper;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Queries.GetById;
using Users.Application.Features.Users.Queries.GetUserById;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Queries.GetUserById;

public class GetUserByIdQueryValidatorUnitTests : BaseUsersUnitTest
{
	private readonly GetUserByIdQueryValidator _validator;

	public GetUserByIdQueryValidatorUnitTests()
	{
		_validator = new GetUserByIdQueryValidator();
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(UsersBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForId_WhenIdLengthIsInvalid(int length)
	{
		// Arrange
		var query = new GetUserByIdQuery(SharedTestUtilities.GetString(length));
		// Act
		var result = _validator.TestValidate(query);
		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Id);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenModelIsValid()
	{
		// Arrange
		var query = new GetUserByIdQuery(UsersTestUtilities.ValidId);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldNotHaveValidationErrorFor(m => m.Id);
	}
}
