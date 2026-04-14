using Shared.Domain.Enums;

namespace Users.Users.Models.Requests;

public sealed record RegisterUserByAdminRequest(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	DateTime DateOfBirth,
	string PhoneNumber,
	string Address,
	Roles Role,
	bool EmailVerified = false
);
