using FluentAssertions;
using NSubstitute;
using Users.Application.Managers.Interfaces;
using Users.Application.Services;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Result;
using Xunit;

namespace Users.Application.UnitTests.Services.EmailServiceTests
{
	public class EmailServiceTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly EmailService _emailService;
		private readonly EmailVerificationToken _validToken;
		private readonly User _user;

		public EmailServiceTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_emailService = new EmailService(_mockRepositoryManager);

			_user = new User("123", "test@example.com", "hashedPassword", "salt", "UserRole", "John", "Doe", DateTime.UtcNow, "1234567890", "Address", false);
			_validToken = new EmailVerificationToken
			(
				"token123",
				 _user.Id,
				DateTime.UtcNow,
				DateTime.UtcNow.AddHours(1) ,
				 _user
			);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnFailure_WhenTokenIsInvalid()
		{
			// Arrange
			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync("invalidToken")
				.Returns(Result<EmailVerificationToken>.Failure(Response.InvalidVerificationToken));

			// Act
			var result = await _emailService.HandleAsync("invalidToken");

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.InvalidVerificationToken);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnFailure_WhenTokenIsExpired()
		{
			// Arrange
			var expiredToken = new EmailVerificationToken
			(
				"token123",
				 _user.Id,
				DateTime.UtcNow,
				DateTime.UtcNow.AddHours(-1),
				 _user
			);

			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync(expiredToken.Id)
				.Returns(Result<EmailVerificationToken>.Success(expiredToken));

			// Act
			var result = await _emailService.HandleAsync(expiredToken.Id);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.InvalidVerificationToken);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnFailure_WhenEmailIsAlreadyVerified()
		{
			// Arrange
			var verifiedUserToken = _validToken;
			verifiedUserToken.User.EmailVerified = true;
			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync(verifiedUserToken.Id)
				.Returns(Result<EmailVerificationToken>.Success(verifiedUserToken));

			// Act
			var result = await _emailService.HandleAsync(verifiedUserToken.Id);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.InvalidVerificationToken);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnSuccess_WhenTokenIsValidAndEmailNotVerified()
		{
			// Arrange
			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync(_validToken.Id)
				.Returns(Result<EmailVerificationToken>.Success(_validToken));

			_mockRepositoryManager.User.VerifyEmailAsync(_validToken.User)
				.Returns(Task.CompletedTask);

			// Act
			var result = await _emailService.HandleAsync(_validToken.Id);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.Ok);
			await _mockRepositoryManager.User.Received(1).VerifyEmailAsync(_validToken.User);
		}
	}

}
