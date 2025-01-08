using Contracts.Enums;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Auth.PasswordManager;
using Users.Application.Commands.Users.RegisterUser;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Infrastructure.MessageBroker;
using Xunit;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class RegisterUserCommandHandlerTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly IPasswordManager _mockPasswordManager;
		private readonly IEmailVerificationSender _mockEmailVerificationSender;
		private readonly IFactoryManager _mockFactoryManager;
		private readonly IEventBus _mockEventBus;

		private readonly RegisterUserCommandHandler _commandHandler;
		private readonly RegisterUserCommand _registerCommand;
		private readonly User _user;

		private string _salt = "salt";
		private readonly string _hashedPassword = "hashedPassword";

		public RegisterUserCommandHandlerTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_mockPasswordManager = Substitute.For<IPasswordManager>();
			_mockEmailVerificationSender = Substitute.For<IEmailVerificationSender>();
			_mockFactoryManager = Substitute.For<IFactoryManager>();
			_mockEventBus = Substitute.For<IEventBus>();

			_commandHandler = new RegisterUserCommandHandler(
				_mockRepositoryManager,
				_mockEmailVerificationSender,
				_mockFactoryManager,
				_mockPasswordManager,
				_mockEventBus
			);

			_registerCommand = new RegisterUserCommand(
				Email: "newuser@example.com",
				Password: "Password",
				FirstName: "John",
				LastName: "Doe",
				DateOfBirth: DateTime.UtcNow.AddYears(-30),
				PhoneNumber: "1234567890",
				Address: "123 Main St",
				Role: Roles.Patient
			);

			_user = new User(
				"1",
				_registerCommand.Email,
				_hashedPassword,
				_salt,
				_registerCommand.Role,
				_registerCommand.FirstName,
				_registerCommand.LastName,
				_registerCommand.DateOfBirth,
				_registerCommand.PhoneNumber,
				_registerCommand.Address,
				true
			);
		}

		[Fact]
		public async Task Handle_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerCommand.Email)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			_mockPasswordManager.HashPassword(_registerCommand.Password, out _salt)
				.Returns(_hashedPassword);

			_mockFactoryManager.UserFactory.CreateUser(
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTime>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<Roles>()
			).Returns(_user);

			_mockEmailVerificationSender.SendEmailAsync(_user)
				.Returns(Result.Success(Responses.RegistrationSuccessful));

			var command = _registerCommand;

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenEmailIsTaken()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerCommand.Email)
				.Returns(Result<User>.Success(_user));

			var command = _registerCommand;

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.EmailTaken);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenEmailSendingFails()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerCommand.Email)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			_mockPasswordManager.HashPassword(_registerCommand.Password, out _salt)
				.Returns(_hashedPassword);

			_mockFactoryManager.UserFactory.CreateUser(
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTime>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<Roles>()
			).Returns(_user);

			_mockEmailVerificationSender.SendEmailAsync(_user)
				.Returns(Result.Failure(Responses.InternalError));

			var command = _registerCommand;

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.InternalError);
		}
	}

}
