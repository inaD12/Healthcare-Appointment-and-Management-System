namespace Appointments.Application.Features.Appointments.Models;

public sealed record AppointmentPaginatedQueryViewModel(
	ICollection<AppointmentQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);
