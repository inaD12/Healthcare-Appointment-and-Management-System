using Patients.Application.Features.Encounters.Dtos;
using Patients.Domain.Entities;
using Shared.Application.Abstractions;

namespace Patients.Application.Features.Encounters.Abstractions;

public interface IEncounterQueryRepository : IGenericReadRepository<Encounter>
{
    Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default);
    Task<List<EncounterListItemDto>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken);

    Task<List<AddendumDto>> GetAddendumsByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<List<DiagnosisDto>> GetDiagnosesByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<List<NoteDto>> GetNotesByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<List<PrescriptionDto>> GetPrescriptionsByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<List<EncounterDetailsDto>> GetDetailsByAppointmentIdsAsync(
        IReadOnlyList<string> appointmentIds,
        CancellationToken cancellationToken);
}
