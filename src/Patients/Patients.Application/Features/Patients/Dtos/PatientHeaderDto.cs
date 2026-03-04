namespace Patients.Application.Features.Patients.Dtos;

public sealed record PatientHeaderDto(
    string Id,
    string FullName,
    DateOnly BirthDate,
    List<string> Allergies,
    List<string> Conditions);
