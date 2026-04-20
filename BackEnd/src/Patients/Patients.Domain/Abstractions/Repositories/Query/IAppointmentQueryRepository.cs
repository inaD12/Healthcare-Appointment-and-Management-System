using Patients.Domain.Entities;

namespace Patients.Domain.Abstractions.Repositories.Query;

public interface IAppointmentQueryRepository
{
    Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct);

    Task<List<AppointmentProjection>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken);
}