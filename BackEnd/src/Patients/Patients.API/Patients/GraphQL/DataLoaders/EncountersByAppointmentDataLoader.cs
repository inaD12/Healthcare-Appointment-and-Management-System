using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Dtos;

namespace Patients.API.Patients.GraphQL.Queries.DataLoaders;

public sealed class EncountersByAppointmentDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterQueryRepository encounterQueryRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, EncounterDetailsDto?>(
        batchScheduler,
        options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, EncounterDetailsDto?>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var encounters = await encounterQueryRepository
            .GetDetailsByAppointmentIdsAsync(keys, cancellationToken);

        return encounters.ToDictionary(
            e => e.AppointmentId,
            e => e
        )!;
    }
}