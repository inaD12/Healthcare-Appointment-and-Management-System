using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	DateTime DateOfBirth,
	string PhoneNumber,
	string Address,
	Roles Role) : ICommand<UserCommandViewModel>;
