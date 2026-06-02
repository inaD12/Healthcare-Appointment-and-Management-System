using Patients.Application.Features.Encounters.Abstractions;
using Patients.Application.Features.Encounters.Dtos;

namespace Patients.API.Patients.GraphQL.DataLoaders;

public sealed class EncountersByPatientDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterQueryRepository encounterQueryRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<EncounterListItemDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<EncounterListItemDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var list = await encounterQueryRepository.GetByPatientIdsAsync(keys, cancellationToken);

        return list
            .GroupBy(e => e.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}