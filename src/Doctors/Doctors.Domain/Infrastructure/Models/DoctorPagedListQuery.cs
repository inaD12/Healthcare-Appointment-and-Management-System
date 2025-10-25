using Shared.Domain.Enums;

namespace Doctors.Domain.Infrastructure.Models;

public sealed record DoctorPagedListQuery(
    string? FirstName,
    string? LastName,
    string? Speciality,
    string? TimeZoneId,
    SortOrder SortOrder,
    string SortPropertyName,
    int Page,
    int PageSize
);
