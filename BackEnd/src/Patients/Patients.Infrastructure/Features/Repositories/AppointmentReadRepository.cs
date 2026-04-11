using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Infrastructure.Features.Repositories;

internal sealed class AppointmentReadRepository(PatientsDbContext db) : IAppointmentReadRepository
{
    public async Task<AppointmentProjection?> GetAsync(string id, CancellationToken ct)
    {
        return await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task UpsertAsync(AppointmentProjection projection, CancellationToken ct)
    {
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
        var entity = await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return;

        update(entity);

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(string id, CancellationToken ct)
    {
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
        var res = await db.AppointmentProjections
            .AsNoTracking()
            .Where(a => patientIds.Contains(a.PatientId))
            .ToListAsync(cancellationToken);
        
        return res;
    }

    public IQueryable<AppointmentProjection> GetByPatient(string patientId)
    {
        var res = db.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.PatientId == patientId);

        return res;
    }

    public IQueryable<AppointmentProjection> GetByDoctor(string doctorId)
    {
        var res = db.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.DoctorId == doctorId);

        return res;
    }

    public IQueryable<AppointmentProjection> GetById(string appointmentId)
    {
        var res = db.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.Id == appointmentId);

        return res;
    }
}