using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories;

public interface IEncounterRepository : IGenericRepository<Encounter>
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
