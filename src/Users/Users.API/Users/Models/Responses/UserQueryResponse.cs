using Shared.Domain.Enums;

namespace Users.Users.Models.Responses;

public sealed record UserQueryResponse(
	string Id,
	string Email,
	Roles Role,
	string FirstName,
	string LastName,
	string PhoneNumber,
	string Address,
	bool EmailVerified);
