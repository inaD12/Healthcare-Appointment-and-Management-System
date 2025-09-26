using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Users.Models.Responses;

public sealed record UserQueryResponse(
	string Id,
	string Email,
	List<Role> Role,
	string FirstName,
	string LastName,
	string PhoneNumber,
	string Address,
	bool EmailVerified);
