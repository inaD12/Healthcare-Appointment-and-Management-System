using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class EncountersByAppointmentDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository encounterRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, EncounterDetailsDto?>(
        batchScheduler,
        options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, EncounterDetailsDto?>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var encounters = await encounterRepository
            .GetDetailsByAppointmentIdsAsync(keys, cancellationToken);

        return encounters.ToDictionary(
            e => e.AppointmentId,
            e => e
        )!;
    }
}