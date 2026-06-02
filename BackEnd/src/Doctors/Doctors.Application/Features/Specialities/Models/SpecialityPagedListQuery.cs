using Shared.Domain.Enums;

namespace Doctors.Application.Features.Specialities.Models;

public sealed record SpecialityPagedListQuery(
    string? Name,
    string? Description,
    SortOrder SortOrder,
    string SortPropertyName,
    int Page,
    int PageSize
);
