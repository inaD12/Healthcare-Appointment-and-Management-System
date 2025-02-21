using FluentAssertions;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Commands.UserCommandHandlerTests;

public class RegisterUserIntegrationTests : BaseUsersIntegrationTest
{
	public RegisterUserIntegrationTests(IntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task SendAsync_ShouldReturnFailure_WhenEmailIsTaken()
	{
		// Arrange
		string email = await CreateUserAsync(); ;

		var command = new RegisterUserCommand(
				UsersTestUtilities.TakenEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var res = await Sender.Send(command);

		// Assert
		res.IsFailure.Should().BeTrue();
		res.Response.Should().BeEquivalentTo(Responses.EmailTaken);
	}

	[Fact]
	public async Task SendAsync_ShouldRegisterUser_WhenEmalIsValid()
	{
		// Arrange
		var command = new RegisterUserCommand(
				UsersTestUtilities.ValidEmail,
				UsersTestUtilities.ValidPassword,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate,
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				Role: Roles.Doctor
			);

		// Act
		var response = await Sender.Send(command, CancellationToken);
		var res = await RepositoryManager.User.GetByEmailAsync(command.Email);
		var user = res.Value;

		// Assert
		user
			.Should()
			.Match<User>(p => p.Email == command.Email &&
							p.FirstName == command.FirstName &&
							p.LastName == command.LastName &&
							p.PhoneNumber == command.PhoneNumber &&
							p.DateOfBirth == command.DateOfBirth &&
							p.Address == command.Address &&
							p.Role == command.Role &&
							PasswordManager.VerifyPassword(command.Password, p.PasswordHash, p.Salt));
	}
}
