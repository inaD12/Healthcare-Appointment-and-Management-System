using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Encounters.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class DiagnosesByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    PatientsReadDbContext db,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<DiagnosisDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<DiagnosisDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var diagnoses = await db.Encounters
            .AsNoTracking()
            .Where(e => keys.Contains(e.Id))
            .SelectMany(e => e.Diagnoses.Select(d => new { e.Id, Diagnosis = new DiagnosisDto(d.Id, d.IcdCode, d.Description) }))
            .ToListAsync(cancellationToken);

        return diagnoses
            .GroupBy(d => d.Id)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Diagnosis).ToList());
    }
}