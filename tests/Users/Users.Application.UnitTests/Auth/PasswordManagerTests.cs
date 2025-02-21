using FluentAssertions;
using Users.Application.Features.Auth;
using Users.Domain.Utilities;
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
		string salt1, salt2;
		string hash1 = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword, out salt1);
		string hash2 = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword, out salt2);

		// Assert
		hash1.Should().NotBe(hash2, "Hashes should be unique for each hashing attempt.");
		salt1.Should().NotBe(salt2, "Salts should be unique for each hashing attempt.");
	}

	[Fact]
	public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
	{
		// Arrange
		string salt;
		string hash = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword, out salt);

		// Act
		bool isPasswordValid = _passwordManager.VerifyPassword(UsersTestUtilities.ValidPassword, hash, salt);

		// Assert
		isPasswordValid.Should().BeTrue("The password should be verified successfully.");
	}

	[Fact]
	public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
	{
		// Arrange
		string salt;
		string hash = _passwordManager.HashPassword(UsersTestUtilities.ValidPassword, out salt);

		// Act
		bool isPasswordValid = _passwordManager.VerifyPassword(UsersTestUtilities.InvalidPassword, hash, salt);

		// Assert
		isPasswordValid.Should().BeFalse("The password verification should fail for an incorrect password.");
	}
}
