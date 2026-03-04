namespace Patients.Infrastructure.Features.ReadModels.Abstractions;

public interface IAppointmentReadRepository
{
    Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct);

    Task UpsertAsync(AppointmentProjection projection, CancellationToken ct);

    Task UpdateAsync(string id, Action<AppointmentProjection> update, CancellationToken ct);

    Task RemoveAsync(string id, CancellationToken ct);
}