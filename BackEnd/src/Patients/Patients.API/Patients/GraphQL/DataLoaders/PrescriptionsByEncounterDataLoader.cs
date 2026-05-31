using Patients.Application.Features.Encounters.Abstractions;
using Patients.Application.Features.Encounters.Dtos;

namespace Patients.API.Patients.GraphQL.DataLoaders;

public sealed class PrescriptionsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterQueryRepository repo,
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