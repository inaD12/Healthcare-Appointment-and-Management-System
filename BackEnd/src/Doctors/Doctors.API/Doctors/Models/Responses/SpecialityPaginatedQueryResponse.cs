using Doctors.Application.Features.Specialities.Models;

namespace Doctors.API.Doctors.Models.Responses;

public sealed record SpecialityPaginatedQueryResponse
(
	ICollection<SpecialityQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage
);
