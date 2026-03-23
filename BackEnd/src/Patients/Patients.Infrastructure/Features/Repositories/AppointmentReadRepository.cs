using System.Data.Entity;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Patients.Infrastructure.Features.ReadModels;

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
    
    public async Task<List<AppointmentHistoryDto>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken)
    {
        return await db.AppointmentProjections
            .AsNoTracking()
            .Where(a => patientIds.Contains(a.PatientId))
            .Select(a => new AppointmentHistoryDto(
                a.Id,
                a.Start,
                a.End,
                a.Status,
                a.DoctorId,
                a.PatientId))
            .ToListAsync(cancellationToken);
    }

    public IQueryable<AppointmentHistoryDto> GetByPatient(string patientId)
    {
        return db.AppointmentProjections
            .AsNoTracking()
            .Where(a => a.PatientId == patientId)
            .Select(a => new AppointmentHistoryDto(
                a.Id,
                a.Start,
                a.End,
                a.Status,
                a.DoctorId,
                a.PatientId));
    }

    public IQueryable<AppointmentHistoryDto> GetByDoctor(string doctorId)
    {
        return db.AppointmentProjections
            .AsNoTracking()
            .Where(a => a.DoctorId == doctorId)
            .Select(a => new AppointmentHistoryDto(
                a.Id,
                a.Start,
                a.End,
                a.Status,
                a.DoctorId,
                a.PatientId));
    }

    public IQueryable<AppointmentHistoryDto> GetById(string appointmentId)
    {
        return db.AppointmentProjections
            .AsNoTracking()
            .Where(a => a.Id == appointmentId)
            .Select(a => new AppointmentHistoryDto(
                a.Id,
                a.Start,
                a.End,
                a.Status,
                a.DoctorId,
                a.PatientId));
    }
}