using Shared.Domain.Entities;

namespace Users.Application.Features.Users.Models;

public sealed record UserQueryViewModel(
	string Id,
	string Email,
	IReadOnlyCollection<Role> Roles,
	string FirstName,
	string LastName,
	string PhoneNumber,
	string Address,
	bool EmailVerified);
