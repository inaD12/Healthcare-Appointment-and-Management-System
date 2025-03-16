using FluentAssertions;
using NSubstitute;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;
using Xunit;

namespace Users.Application.UnitTests.Commands.EmailCommandHandlerTests;

public class EmailCommandHandlerTests
{
	private readonly IEmailVerificationTokenRepository _mockEmailVerificationTokenRepository;
	private readonly IUserRepository _mockUserRepository;
	private readonly HandleEmailCommandHandler _commandHandler;
	private readonly EmailVerificationToken _validToken;
	private readonly User _user;
	private readonly IUnitOfWork _unitOfWork;

	public EmailCommandHandlerTests()
	{
		_mockEmailVerificationTokenRepository = Substitute.For<IEmailVerificationTokenRepository>();
		_mockUserRepository = Substitute.For<IUserRepository>();
		_unitOfWork = Substitute.For<IUnitOfWork>();
		_commandHandler = new HandleEmailCommandHandler(_unitOfWork, _mockEmailVerificationTokenRepository, _mockUserRepository);

		_user =  User.Create("test@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address");
		_validToken = new EmailVerificationToken
		(
			 _user.Id,
			DateTime.UtcNow,
			DateTime.UtcNow.AddHours(1),
			 _user
		);
	}

	[Fact]
	public async Task HandleAsync_ShouldReturnFailure_WhenTokenIsInvalid()
	{
		// Arrange
		var command = new HandleEmailCommand("invalidToken");
		_mockEmailVerificationTokenRepository.GetByIdAsync("invalidToken")
			.Returns(Result<EmailVerificationToken>.Failure(ResponseList.InvalidVerificationToken));

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.InvalidVerificationToken);
	}

	[Fact]
	public async Task HandleAsync_ShouldReturnFailure_WhenTokenIsExpired()
	{
		// Arrange
		var expiredToken = new EmailVerificationToken
		(
			 _user.Id,
			DateTime.UtcNow,
			DateTime.UtcNow.AddHours(-1),
			 _user
		);

		var command = new HandleEmailCommand(expiredToken.Id);

		_mockEmailVerificationTokenRepository.GetByIdAsync(expiredToken.Id)
			.Returns(Result<EmailVerificationToken>.Success(expiredToken));

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.InvalidVerificationToken);
	}

	[Fact]
	public async Task HandleAsync_ShouldReturnFailure_WhenEmailIsAlreadyVerified()
	{
		// Arrange
		var verifiedUserToken = _validToken;
		verifiedUserToken.User.VerifyEmail();
		_mockEmailVerificationTokenRepository.GetByIdAsync(verifiedUserToken.Id)
			.Returns(Result<EmailVerificationToken>.Success(verifiedUserToken));
		var command = new HandleEmailCommand(verifiedUserToken.Id);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.InvalidVerificationToken);
	}

	[Fact]
	public async Task HandleAsync_ShouldReturnSuccess_WhenTokenIsValidAndEmailNotVerified()
	{
		// Arrange
		var command = new HandleEmailCommand(_validToken.Id);

		_mockEmailVerificationTokenRepository.GetByIdAsync(_validToken.Id)
			.Returns(Result<EmailVerificationToken>.Success(_validToken));


		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Response.Ok);
		//_mockUserRepository.Received(1).VerifyEmailAsync(_validToken.User);
	}
}
