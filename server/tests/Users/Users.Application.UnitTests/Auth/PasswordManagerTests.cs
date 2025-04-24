using FluentAssertions;
using Users.Domain.Utilities;
using Users.Infrastructure.Features.Auth;
using Xunit;

namespace Users.Application.UnitTests.Auth;

public class PasswordManagerTests : BaseUsersUnitTest
{
	private readonly PasswordManager _passwordManager;

	public PasswordManagerTests()
	{
		_passwordManager = new PasswordManager();
	}

	[Fact]
	public void HashPassword_ShouldGenerateUniqueHashAndSalt()
	{
		// Arrange

		// Act
		var hashRes1 = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword);
		var hashRes2 = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword);

		// Assert
		hashRes1.PasswordHash.Should().NotBe(hashRes2.PasswordHash, "Hashes should be unique for each hashing attempt.");
		hashRes1.Salt.Should().NotBe(hashRes2.Salt, "Salts should be unique for each hashing attempt.");
	}

	[Fact]
	public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
	{
		// Arrange
		var hashRes = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword);

		// Act
		bool isPasswordValid = _passwordManager.VerifyPassword(UsersTestUtilities.ValidPassword, hashRes.PasswordHash, hashRes.Salt);

		// Assert
		isPasswordValid.Should().BeTrue("The password should be verified successfully.");
	}

	[Fact]
	public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
	{
		// Arrange
		var hashRes = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword);

		// Act
		bool isPasswordValid = _passwordManager.VerifyPassword(UsersTestUtilities.InvalidPassword, hashRes.PasswordHash, hashRes.Salt);

		// Assert
		isPasswordValid.Should().BeFalse("The password verification should fail for an incorrect password.");
	}
}
