using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Encounters.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class PrescriptionsByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    PatientsReadDbContext db,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<PrescriptionDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<PrescriptionDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var prescriptions = await db.Encounters
            .AsNoTracking()
            .Where(e => keys.Contains(e.Id))
            .SelectMany(e => e.Prescriptions.Select(p => new { e.Id, Prescription = new PrescriptionDto(p.Id, p.MedicationName, p.Dosage, p.Instructions) }))
            .ToListAsync(cancellationToken);

        return prescriptions
            .GroupBy(p => p.Id)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Prescription).ToList());
    }
}