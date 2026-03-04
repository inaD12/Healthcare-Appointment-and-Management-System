using Shared.Domain.Enums;

namespace Users.Users.Models.Requests;

public sealed record RegisterUserRequest(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	DateTime DateOfBirth,
	string PhoneNumber,
	string Address,
	Roles Role
);
