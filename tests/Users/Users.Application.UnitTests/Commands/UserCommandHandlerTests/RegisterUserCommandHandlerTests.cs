using FluentAssertions;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Commands.UserCommandHandlerTests;

public class RegisterUserCommandHandlerTests : BaseUsersUnitTest
{
	private readonly RegisterUserCommandHandler _commandHandler;

	public RegisterUserCommandHandlerTests()
	{
		_commandHandler = new RegisterUserCommandHandler(
			PasswordManager,
			HAMSMapper,
			UnitOfWork,
			UserRepository
			);
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
	{
		//Arramge
		var command = new RegisterUserCommand(
			UsersTestUtilities.UnusedEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Role: Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenEmailIsTaken()
	{
		// Arrange
		var command = new RegisterUserCommand(
			UsersTestUtilities.TakenEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Role: Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.EmailTaken);
	}
}
