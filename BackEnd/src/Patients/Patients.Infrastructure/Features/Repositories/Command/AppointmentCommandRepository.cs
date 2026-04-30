using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Infrastructure.Features.Repositories.Command;

internal sealed class AppointmentCommandRepository(PatientsCommandDbContext context) : IAppointmentCommandRepository
{
    public async Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct)
    {
        return await context.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task UpsertAsync(AppointmentProjection projection, CancellationToken ct)
    {
        var existing = await context.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == projection.Id, ct);

        if (existing is null)
        {
            await context.AppointmentProjections.AddAsync(projection, ct);
        }
        else
        {
            context.Entry(existing).CurrentValues.SetValues(projection);
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(string id, Action<AppointmentProjection> update, CancellationToken ct)
    {
        var entity = await context.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return;

        update(entity);

        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(string id, CancellationToken ct)
    {
        var entity = await context.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return;

        context.AppointmentProjections.Remove(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task<List<AppointmentProjection>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken)
    {
        return await context.AppointmentProjections
            .AsNoTracking()
            .Where(a => patientIds.Contains(a.PatientId))
            .ToListAsync(cancellationToken);
    }
}