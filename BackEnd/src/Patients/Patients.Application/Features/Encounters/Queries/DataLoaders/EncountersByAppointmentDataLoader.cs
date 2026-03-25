using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class EncountersByAppointmentDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository encounterRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<EncounterDetailsDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<EncounterDetailsDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var list = await encounterRepository.GetDetailsByAppointmentIdsAsync(keys, cancellationToken);

        return list
            .GroupBy(e => e.Id)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}