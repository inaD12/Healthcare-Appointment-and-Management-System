using FluentAssertions;
using Users.Application.Auth.PasswordManager;
using Xunit;

namespace Users.Application.UnitTests.Auth
{
	public class PasswordManagerTests
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
			string password = "MySecurePassword";

			// Act
			string salt1, salt2;
			string hash1 = _passwordManager.HashPassword(password, out salt1);
			string hash2 = _passwordManager.HashPassword(password, out salt2);

			// Assert
			hash1.Should().NotBe(hash2, "Hashes should be unique for each hashing attempt.");
			salt1.Should().NotBe(salt2, "Salts should be unique for each hashing attempt.");
		}

		[Fact]
		public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
		{
			// Arrange
			string password = "MySecurePassword";
			string salt;
			string hash = _passwordManager.HashPassword(password, out salt);

			// Act
			bool isPasswordValid = _passwordManager.VerifyPassword(password, hash, salt);

			// Assert
			isPasswordValid.Should().BeTrue("The password should be verified successfully.");
		}

		[Fact]
		public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
		{
			// Arrange
			string correctPassword = "MySecurePassword";
			string incorrectPassword = "WrongPassword";
			string salt;
			string hash = _passwordManager.HashPassword(correctPassword, out salt);

			// Act
			bool isPasswordValid = _passwordManager.VerifyPassword(incorrectPassword, hash, salt);

			// Assert
			isPasswordValid.Should().BeFalse("The password verification should fail for an incorrect password.");
		}
	}

}
