using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Application.Features.Users.Models;

public sealed record UserPagedListQuery(
	string? Email,
	Role? Role,
	string? FirstName,
	string? LastName,
	string? PhoneNumber,
	string? Address,
	bool? EmailVerified,
	SortOrder SortOrder,
	string SortPropertyName,
	int Page,
	int PageSize);
