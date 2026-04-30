namespace Ratings.Application.Features.Ratings.Models;

public sealed record RatingPaginatedQueryViewModel(
	ICollection<RatingQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);
