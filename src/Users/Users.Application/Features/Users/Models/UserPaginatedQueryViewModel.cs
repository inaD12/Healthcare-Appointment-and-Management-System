namespace Users.Application.Features.Users.Models;

public sealed record UserPaginatedQueryViewModel(
	ICollection<UserQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);
