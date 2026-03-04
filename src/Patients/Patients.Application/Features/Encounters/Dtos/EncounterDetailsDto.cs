using Patients.Domain.Enums;

namespace Patients.Application.Features.Encounters.Dtos;

public sealed record EncounterDetailsDto(
    string Id,
    DateTime StartedAt,
    DateTime? FinalizedAt,
    EncounterStatus Status,
    List<NoteDto> Notes,
    List<DiagnosisDto> Diagnoses,
    List<PrescriptionDto> Prescriptions,
    List<AddendumDto> Addendums);
