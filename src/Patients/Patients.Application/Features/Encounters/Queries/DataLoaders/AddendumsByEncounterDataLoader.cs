using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Encounters.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class AddendumsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    PatientsReadDbContext db,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<AddendumDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<AddendumDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var addendums = await db.Encounters
            .AsNoTracking()
            .Where(e => keys.Contains(e.Id))
            .SelectMany(e => e.Addendums.Select(a => new { e.Id, Addendum = new AddendumDto(a.Id, a.Text, a.CreatedAt) }))
            .ToListAsync(cancellationToken);

        return addendums
            .GroupBy(a => a.Id)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Addendum).ToList());
    }
}
