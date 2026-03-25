using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class NotesByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository encounterRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<NoteDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<NoteDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        return await encounterRepository.GetNotesByEncounterIdsAsync(keys, cancellationToken);
    }
}