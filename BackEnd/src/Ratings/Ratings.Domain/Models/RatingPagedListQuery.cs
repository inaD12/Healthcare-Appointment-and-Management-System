using Shared.Domain.Enums;

namespace Ratings.Domain.Models;

public sealed record RatingPagedListQuery(
    string? PatientId,
    string? DoctorId,
    string? AppointmentId,
    int? MinScore,
    int? MaxScore,
    SortOrder SortOrder,
    string SortPropertyName,
    int Page,
    int PageSize);