using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.AppointmentProjections.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.AppointmentProjections.Queries.DataLoaders;

public sealed class AppointmentsByPatientDataLoader(
    IBatchScheduler batchScheduler,
    PatientsReadDbContext db,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<AppointmentHistoryDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<AppointmentHistoryDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var appointments = await db.AppointmentProjections
            .AsNoTracking()
            .Where(a => keys.Contains(a.PatientId))
            .Select(a => new AppointmentHistoryDto(
                a.Id,
                a.Start,
                a.End,
                a.Status,
                a.DoctorId,
                a.PatientId))
            .ToListAsync(cancellationToken);

        return appointments
            .GroupBy(a => a.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}