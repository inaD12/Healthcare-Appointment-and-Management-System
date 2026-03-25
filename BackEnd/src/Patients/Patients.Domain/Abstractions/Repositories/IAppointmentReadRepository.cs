using Patients.Domain.Dtos;
using Patients.Domain.Entities;

namespace Patients.Domain.Abstractions.Repositories;

public interface IAppointmentReadRepository
{
    Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct);

    Task UpsertAsync(AppointmentProjection projection, CancellationToken ct);

    Task UpdateAsync(string id, Action<AppointmentProjection> update, CancellationToken ct);

    Task RemoveAsync(string id, CancellationToken ct);
    
    Task<List<AppointmentHistoryDto>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken);

    IQueryable<AppointmentHistoryDto> GetByPatient(string patientId);
    IQueryable<AppointmentHistoryDto> GetByDoctor(string doctorId);
    IQueryable<AppointmentHistoryDto> GetById(string appointmentId);
}