using Contracts.Enums;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using Users.Application.Commands.Email.HandleEmail;
using Users.Application.Managers.Interfaces;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Xunit;

namespace Users.Application.UnitTests.Services.EmailServiceTests
{
	public class EmailServiceTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly HandleEmailCommandHandler _commandHandler;
		private readonly EmailVerificationToken _validToken;
		private readonly User _user;

		public EmailServiceTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_commandHandler = new HandleEmailCommandHandler(_mockRepositoryManager);

			_user = new User("123", "test@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address", false);
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
			var command = new HandleEmailCommand("invalidToken");
			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync("invalidToken")
				.Returns(Result<EmailVerificationToken>.Failure(Responses.InvalidVerificationToken));

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.InvalidVerificationToken);
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

			var command = new HandleEmailCommand(expiredToken.Id);

			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync(expiredToken.Id)
				.Returns(Result<EmailVerificationToken>.Success(expiredToken));

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.InvalidVerificationToken);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnFailure_WhenEmailIsAlreadyVerified()
		{
			// Arrange
			var verifiedUserToken = _validToken;
			verifiedUserToken.User.EmailVerified = true;
			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync(verifiedUserToken.Id)
				.Returns(Result<EmailVerificationToken>.Success(verifiedUserToken));
			var command = new HandleEmailCommand(verifiedUserToken.Id);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.InvalidVerificationToken);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnSuccess_WhenTokenIsValidAndEmailNotVerified()
		{
			// Arrange
			var command = new HandleEmailCommand(_validToken.Id);

			_mockRepositoryManager.EmailVerificationToken.GetTokenByIdAsync(_validToken.Id)
				.Returns(Result<EmailVerificationToken>.Success(_validToken));

			_mockRepositoryManager.User.VerifyEmailAsync(_validToken.User)
				.Returns(Task.CompletedTask);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.Ok);
			await _mockRepositoryManager.User.Received(1).VerifyEmailAsync(_validToken.User);
		}
	}

}
