using Shared.Domain.Enums;

namespace Users.Application.Features.Users.Models;

public sealed record UserQueryViewModel(
	string Id,
	string Email,
	IReadOnlyCollection<Roles> Roles,
	string FirstName,
	string LastName,
	string PhoneNumber,
	string Address,
	bool EmailVerified);
