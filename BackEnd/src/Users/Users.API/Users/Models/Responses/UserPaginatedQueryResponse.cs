using Users.Application.Features.Users.Models;

namespace Users.Users.Models.Responses;

public sealed record UserPaginatedQueryResponse
(
	ICollection<UserQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage
);
