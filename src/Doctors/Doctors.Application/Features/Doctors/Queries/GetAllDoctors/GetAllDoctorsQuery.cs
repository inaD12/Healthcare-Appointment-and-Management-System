using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;

namespace Doctors.Application.Features.Doctors.Queries.GetAllDoctors;

public sealed record GetAllDoctorsQuery(
	string? Speciality,
	string? TimeZoneId,
	SortOrder SortOrder,
	int Page,
	int PageSize,
	string SortPropertyName) : IQuery<DoctorPaginatedQueryViewModel>;
