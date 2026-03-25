using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class PrescriptionsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository encounterRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<PrescriptionDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<PrescriptionDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        return await encounterRepository.GetPrescriptionsByEncounterIdsAsync(keys, cancellationToken);
    }
}