using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories;

public interface IEncounterRepository : IGenericRepository<Encounter>
{
    Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default);
    IQueryable<EncounterListItemDto> GetByPatient(string patientId);
    IQueryable<EncounterDetailsDto> GetDetails(string encounterId);
    Task<List<EncounterListItemDto>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken);
    Task<Dictionary<string, List<AddendumDto>>> GetAddendumsByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<Dictionary<string, List<DiagnosisDto>>> GetDiagnosesByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<Dictionary<string, List<NoteDto>>> GetNotesByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<Dictionary<string, List<PrescriptionDto>>> GetPrescriptionsByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken);

    Task<List<EncounterDetailsDto>> GetDetailsByAppointmentIdsAsync(
        IReadOnlyList<string> appointmentIds,
        CancellationToken cancellationToken);
}
