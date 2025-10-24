using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(
	string? Email,
	Role? Role,
	string? FirstName,
	string? LastName,
	string? PhoneNumber,
	string? Address,
	bool? EmailVerified,
	SortOrder? SortOrder,
	int Page,
	int PageSize,
	string SortPropertyName) : IQuery<UserPaginatedQueryViewModel>;
