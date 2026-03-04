using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Encounters.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class NotesByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    PatientsReadDbContext db,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<NoteDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<NoteDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var notes = await db.Encounters
            .AsNoTracking()
            .Where(e => keys.Contains(e.Id))
            .SelectMany(e => e.Notes.Select(n => new { e.Id, Note = new NoteDto(n.Id, n.Text, n.CreatedAt) }))
            .ToListAsync(cancellationToken);

        return notes
            .GroupBy(n => n.Id)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Note).ToList());
    }
}