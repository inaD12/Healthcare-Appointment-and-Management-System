using FluentValidation.TestHelper;
using Shared.Domain.Enums;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidatorUnitTests : BaseUsersUnitTest
{
	private readonly GetAllUsersQueryValidator _validator;

	public GetAllUsersQueryValidatorUnitTests()
	{
		_validator = new GetAllUsersQueryValidator();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.EMAIL_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.EMAIL_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForEmail_WhenEmailLengthIsInvalid(int length)
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			SharedTestUtilities.GetString(length),
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Email);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForRole_WhenRoleIsInvalid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			(Roles)99,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Role);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForFirstName_WhenFirstNameLengthIsInvalid(int length)
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			SharedTestUtilities.GetString(length),
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.FirstName);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForLastName_WhenLastNameLengthIsInvalid(int length)
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			SharedTestUtilities.GetString(length),
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.LastName);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForPhoneNumber_WhenPhoneNumberLengthIsInvalid(int length)
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			SharedTestUtilities.GetString(length),
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PhoneNumber);
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.ADRESS_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ADRESS_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForAddress_WhenAddressLengthIsInvalid(int length)
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			SharedTestUtilities.GetString(length),
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Address);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForSortPropertyName_WhenSortPropertyNameIsInvalid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.InvalidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.SortPropertyName);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForSortPropertyName_WhenSortPropertyNameIsNull()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			null!
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.SortPropertyName);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenModelIsValid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenModelParametersAreNull()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			null!,
			null!,
			null!,
			null!,	
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
