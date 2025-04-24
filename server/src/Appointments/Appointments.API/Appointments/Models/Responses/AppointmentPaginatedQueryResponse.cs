using Appointments.Application.Features.Appointments.Models;

namespace Appointments.API.Appointments.Models.Responses;

public sealed record AppointmentPaginatedQueryResponse(
ICollection<AppointmentQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);