using Contracts.Abstractions.Messaging;
using Contracts.Enums;

namespace Users.Application.Commands.Users.RegisterUser;

public sealed record RegisterUserCommand(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	DateTime DateOfBirth,
	string PhoneNumber,
	string Address,
	Roles Role) : ICommand;
