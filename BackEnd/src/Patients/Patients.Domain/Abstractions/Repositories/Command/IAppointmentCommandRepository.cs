using Patients.Domain.Entities;

namespace Patients.Domain.Abstractions.Repositories.Command;

public interface IAppointmentCommandRepository
{
    Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct);

    Task UpsertAsync(AppointmentProjection projection, CancellationToken ct);

    Task UpdateAsync(string id, Action<AppointmentProjection> update, CancellationToken ct);

    Task RemoveAsync(string id, CancellationToken ct);
    
    Task<List<AppointmentProjection>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken);
}