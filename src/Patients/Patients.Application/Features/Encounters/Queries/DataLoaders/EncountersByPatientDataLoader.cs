using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Encounters.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class EncountersByPatientDataLoader(
    IBatchScheduler batchScheduler,
    PatientsReadDbContext db,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<EncounterListItemDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<EncounterListItemDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var encounters = await db.Encounters
            .AsNoTracking()
            .Where(e => keys.Contains(e.PatientId))
            .Select(e => new EncounterListItemDto(
                e.Id,
                e.StartedAt,
                e.Status,
                e.DoctorId,
                e.PatientId))
            .ToListAsync(cancellationToken);

        return encounters
            .GroupBy(e => e.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}