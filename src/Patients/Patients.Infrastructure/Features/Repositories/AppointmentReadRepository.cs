using System.Data.Entity;
using Patients.Infrastructure.Features.DBContexts;
using Patients.Infrastructure.Features.ReadModels;
using Patients.Infrastructure.Features.ReadModels.Abstractions;

namespace Patients.Infrastructure.Features.Repositories;

internal sealed class AppointmentReadRepository(PatientsDbContext db) : IAppointmentReadRepository
{
    public async Task<AppointmentProjection?> GetAsync(string id, CancellationToken cancellationToken)
    {
        return await db.AppointmentProjections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AppointmentProjection>> GetPatientAppointmentsAsync(string patientId, CancellationToken cancellationToken)
    {
        return await db.AppointmentProjections
            .AsNoTracking()
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(x => x.Start)
            .ToListAsync(cancellationToken);
    }

    public async Task AddOrUpdateAsync(AppointmentProjection appointment, CancellationToken cancellationToken)
    {
        var existing = await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == appointment.Id, cancellationToken);

        if (existing is null)
        {
            await db.AppointmentProjections.AddAsync(appointment, cancellationToken);
        }
        else
        {
            existing.PatientId = appointment.PatientId;
            existing.DoctorId = appointment.DoctorId;
            existing.Start = appointment.Start;
            existing.End = appointment.End;
            existing.Status = appointment.Status;
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(string appointmentId, CancellationToken cancellationToken)
    {
        var existing = await db.AppointmentProjections
            .FirstOrDefaultAsync(x => x.Id == appointmentId, cancellationToken);

        if (existing is null)
            return;

        db.AppointmentProjections.Remove(existing);
        await db.SaveChangesAsync(cancellationToken);
    }
}