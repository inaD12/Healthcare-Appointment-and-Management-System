using Patients.Domain.Enums;

namespace Patients.Domain.Dtos;

public sealed record EncounterListItemDto(
    string Id,
    DateTime StartedAt,
    EncounterStatus Status,
    string DoctorId,
    string PatientId);
