using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.AppointmentProjections.Queries.DataLoaders;

public sealed class AppointmentsByPatientDataLoader(
    IBatchScheduler batchScheduler,
    IAppointmentReadRepository repo,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<AppointmentHistoryDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<AppointmentHistoryDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var appointments = await repo.GetByPatientIdsAsync(keys, cancellationToken);

        return appointments
            .GroupBy(a => a.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}