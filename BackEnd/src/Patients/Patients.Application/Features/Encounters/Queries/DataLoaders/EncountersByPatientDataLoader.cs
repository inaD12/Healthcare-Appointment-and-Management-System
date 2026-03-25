using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class EncountersByPatientDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository encounterRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<EncounterListItemDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<EncounterListItemDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var list = await encounterRepository.GetByPatientIdsAsync(keys, cancellationToken);

        return list
            .GroupBy(e => e.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}