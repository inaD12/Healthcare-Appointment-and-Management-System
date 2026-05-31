using Patients.Application.Features.Encounters.Abstractions;
using Patients.Application.Features.Encounters.Dtos;

namespace Patients.API.Patients.GraphQL.DataLoaders;

public sealed class AddendumsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterQueryRepository repo,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<string, AddendumDto>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<ILookup<string, AddendumDto>> LoadGroupedBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var rows = await repo.GetAddendumsByEncounterIdsFlatAsync(keys, cancellationToken);

        return rows.ToLookup(x => x.EncounterId);
    }
}