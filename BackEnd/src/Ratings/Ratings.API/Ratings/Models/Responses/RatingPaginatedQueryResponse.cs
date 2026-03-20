using Ratings.Application.Features.Ratings.Models;

namespace Ratings.API.Ratings.Models.Responses;

public sealed record RatingPaginatedQueryResponse(
ICollection<RatingQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);