using FluentAssertions;
using NSubstitute;
using Users.Application.Helpers;
using Xunit;
using Users.Domain.EmailVerification;
using FluentEmail.Core.Models;
using NSubstitute.ExceptionExtensions;
using Contracts.Results;
using Users.Domain.Responses;

namespace Users.Application.UnitTests.Helpers
{

	public class EmailVerificationSenderTests : BaseUsersUnitTest
	{
		public EmailVerificationSenderTests()
		{
		}

		[Fact]
		public async Task SendEmailAsync_ShouldSendEmailSuccessfully()
		{
			// Arrange
			//var emailVerificationToken = new EmailVerificationToken
			//(
			//	 Guid.NewGuid().ToString(),
			//	_mockUser.Id,
			//	DateTime.UtcNow,
			//	DateTime.UtcNow.AddDays(1),
			//	_mockUser
			//);

		//	_mockEmailTokenFactory.CreateToken(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
		//  .Returns(emailVerificationToken);
		//	_mockEmailLinkFactory.Create(emailVerificationToken).Returns("http://verification-link.com");

		//	_mockFluentEmail.To(_mockUser.Email).Returns(_mockFluentEmail);
		//	_mockFluentEmail.Subject(Arg.Any<string>()).Returns(_mockFluentEmail);
		//	_mockFluentEmail.Body(Arg.Any<string>(), true).Returns(_mockFluentEmail);
		//	_mockFluentEmail.SendAsync().Returns(Task.FromResult(new SendResponse()));

		//	// Act
		//	var result = await _emailVerificationSender.SendEmailAsync(_mockUser);

		//	// Assert
		//	result.Should().BeEquivalentTo(Result.Success());
		//	await _mockFluentEmail.Received(1).SendAsync();
		//	await _mockRepositoryManager.EmailVerificationToken.Received(1).AddTokenAsync(emailVerificationToken);
		//}

		//[Fact]
		//public async Task SendEmailAsync_ShouldReturnFailure_WhenEmailSendingFails()
		//{
		//	// Arrange
		//	var emailVerificationToken = new EmailVerificationToken
		//	(
		//		 Guid.NewGuid().ToString(),
		//		_mockUser.Id,
		//		DateTime.UtcNow,
		//		DateTime.UtcNow.AddDays(1),
		//		_mockUser
		//	);

		//	_mockEmailTokenFactory.CreateToken(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
		//		.Returns(emailVerificationToken);
		//	_mockEmailLinkFactory.Create(emailVerificationToken).Returns("http://verification-link.com");

		//	_mockFluentEmail.To(_mockUser.Email).Returns(_mockFluentEmail);
		//	_mockFluentEmail.Subject(Arg.Any<string>()).Returns(_mockFluentEmail);
		//	_mockFluentEmail.Body(Arg.Any<string>(), true).Returns(_mockFluentEmail);
		//	_mockFluentEmail.SendAsync().Returns(Task.FromResult(new SendResponse { ErrorMessages = new List<string> { "ewewewew"} }));

		//	// Act
		//	var result = await _emailVerificationSender.SendEmailAsync(_mockUser);

		//	// Assert
		//	result.Should().BeEquivalentTo(Result.Failure(Responses.EmailNotSent));
		//	await _mockFluentEmail.Received(1).SendAsync();
		//	await _mockRepositoryManager.EmailVerificationToken.DidNotReceive().AddTokenAsync(emailVerificationToken);
		//}

		//[Fact]
		//public async Task SendEmailAsync_ShouldReturnFailure_WhenTokenCreationFails()
		//{
		//	// Arrange
		//	_mockEmailTokenFactory.CreateToken(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
		//		.Throws(new Exception("Token creation error"));

		//	// Act
		//	var result = await _emailVerificationSender.SendEmailAsync(_mockUser);

		//	// Assert
		//	result.Should().BeEquivalentTo(Result.Failure(Responses.InternalError));
		//	await _mockFluentEmail.DidNotReceive().SendAsync();
		//	await _mockRepositoryManager.EmailVerificationToken.DidNotReceive().AddTokenAsync(Arg.Any<EmailVerificationToken>());
		}
	}
}

