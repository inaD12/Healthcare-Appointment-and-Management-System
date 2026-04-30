namespace Doctors.Application.Features.Specialities.Models;

public sealed record SpecialityPaginatedQueryViewModel(
	ICollection<SpecialityQueryViewModel> Items,
	int Page,
	int PageSize,
	int TotalCount,
	bool HasNextPage,
	bool HasPreviousPage);
