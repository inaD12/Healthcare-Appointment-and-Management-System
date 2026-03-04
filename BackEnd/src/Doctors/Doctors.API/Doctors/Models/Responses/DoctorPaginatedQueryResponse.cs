using Doctors.Application.Features.Doctors.Models;

namespace Doctors.API.Doctors.Models.Responses;

public sealed record DoctorPaginatedQueryResponse
(
	ICollection<DoctorQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage
);
