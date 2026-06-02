using Patients.Application.Features.Patients.Abstractions;
using Patients.Domain.Entities;

namespace Patients.API.Patients.GraphQL.DataLoaders;

public sealed class AppointmentsByPatientDataLoader(
    IBatchScheduler batchScheduler,
    IAppointmentQueryRepository repo,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<AppointmentProjection>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<AppointmentProjection>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var appointments = await repo.GetByPatientIdsAsync(keys, cancellationToken);

        return appointments
            .GroupBy(a => a.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}