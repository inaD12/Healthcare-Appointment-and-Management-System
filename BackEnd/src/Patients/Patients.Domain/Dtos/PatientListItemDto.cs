namespace Patients.Application.Features.Patients.Dtos;

public sealed record PatientListItemDto(
    string Id,
    string FullName,
    DateOnly BirthDate);
