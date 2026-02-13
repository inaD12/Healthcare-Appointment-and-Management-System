namespace Patients.Infrastructure.Features.ReadModels.Abstractions;

public interface IAppointmentReadRepository
{
    Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct);

    Task<IReadOnlyList<AppointmentProjection>> GetPatientAppointmentsAsync(string patientId, CancellationToken ct);

    Task AddOrUpdateAsync(AppointmentProjection appointment, CancellationToken ct);

    Task RemoveAsync(string appointmentId, CancellationToken ct);
}