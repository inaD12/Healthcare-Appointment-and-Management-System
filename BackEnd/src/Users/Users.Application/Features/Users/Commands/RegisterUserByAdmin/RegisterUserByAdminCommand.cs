using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Entities;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.Commands.RegisterUserByAdmin;

public sealed record RegisterUserByAdminCommand(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	DateTime DateOfBirth,
	string PhoneNumber,
	string Address,
	Role Role,
	bool EmailVerified = false) : ICommand<UserCommandViewModel>;
