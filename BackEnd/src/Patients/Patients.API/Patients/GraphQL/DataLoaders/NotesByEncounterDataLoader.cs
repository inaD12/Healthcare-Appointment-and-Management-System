using Patients.Application.Features.Encounters.Abstractions;
using Patients.Application.Features.Encounters.Dtos;

namespace Patients.API.Patients.GraphQL.DataLoaders;

public sealed class NotesByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterQueryRepository repo,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<string, NoteDto>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<ILookup<string, NoteDto>> LoadGroupedBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var rows = await repo.GetNotesByEncounterIdsFlatAsync(keys, cancellationToken);

        return rows.ToLookup(x => x.EncounterId);
    }
}