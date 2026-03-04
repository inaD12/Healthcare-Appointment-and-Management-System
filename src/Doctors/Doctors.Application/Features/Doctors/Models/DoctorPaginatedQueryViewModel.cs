namespace Doctors.Application.Features.Doctors.Models;

public sealed record DoctorPaginatedQueryViewModel(
	ICollection<DoctorQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);
