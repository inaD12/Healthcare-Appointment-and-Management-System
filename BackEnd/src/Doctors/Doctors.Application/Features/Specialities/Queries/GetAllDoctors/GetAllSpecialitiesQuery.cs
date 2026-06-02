using Doctors.Application.Features.Specialities.Models;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Enums;

namespace Doctors.Application.Features.Specialities.Queries.GetAllDoctors;

public sealed record GetAllSpecialitiesQuery(
	string? Name,
	string? Description,
	SortOrder SortOrder,
	int Page,
	int PageSize,
	string SortPropertyName) : IQuery<SpecialityPaginatedQueryViewModel>;
