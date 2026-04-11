using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class AddendumsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository repo,
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