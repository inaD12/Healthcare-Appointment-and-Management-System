using Shared.Domain.Enums;

namespace Doctors.Application.Features.Doctors.Models;

public sealed record DoctorPagedListQuery(
    string? FirstName,
    string? LastName,
    string? Speciality,
    SortOrder SortOrder,
    string SortPropertyName,
    int Page,
    int PageSize
);
