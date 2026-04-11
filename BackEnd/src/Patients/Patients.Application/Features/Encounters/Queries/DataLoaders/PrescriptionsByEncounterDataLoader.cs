using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class PrescriptionsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository repo,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<string, PrescriptionDto>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<ILookup<string, PrescriptionDto>> LoadGroupedBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var rows = await repo.GetPrescriptionsByEncounterIdsFlatAsync(keys, cancellationToken);

        return rows.ToLookup(x => x.EncounterId);
    }
}