using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Infrastructure.Features.Repositories.Query;

internal sealed class AppointmentQueryRepository(IDbContextFactory<PatientsQueryDbContext> factory)
    : IAppointmentQueryRepository
{
    private PatientsQueryDbContext CreateDb()
        => factory.CreateDbContext();

    public async Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct)
    {
        await using var db = CreateDb();

        return await db.AppointmentProjections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task UpsertAsync(AppointmentProjection projection, CancellationToken ct)
    {
        await using var db = CreateDb();

        var existing = await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == projection.Id, ct);

        if (existing is null)
        {
            await db.AppointmentProjections.AddAsync(projection, ct);
        }
        else
        {
            db.Entry(existing).CurrentValues.SetValues(projection);
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(string id, Action<AppointmentProjection> update, CancellationToken ct)
    {
        await using var db = CreateDb();

        var entity = await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return;

        update(entity);

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(string id, CancellationToken ct)
    {
        await using var db = CreateDb();

        var entity = await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return;

        db.AppointmentProjections.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<List<AppointmentProjection>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken)
    {
        await using var db = CreateDb();

        return await db.AppointmentProjections
            .AsNoTracking()
            .Where(a => patientIds.Contains(a.PatientId))
            .ToListAsync(cancellationToken);
    }
}