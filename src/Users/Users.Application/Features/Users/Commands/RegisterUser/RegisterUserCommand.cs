using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	DateTime DateOfBirth,
	string PhoneNumber,
	string Address,
	Role Role) : ICommand<UserCommandViewModel>;
