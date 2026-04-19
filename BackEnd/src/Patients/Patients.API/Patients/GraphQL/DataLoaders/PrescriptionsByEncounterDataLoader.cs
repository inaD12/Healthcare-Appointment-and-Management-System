using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Dtos;

namespace Patients.API.Patients.GraphQL.Queries.DataLoaders;

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