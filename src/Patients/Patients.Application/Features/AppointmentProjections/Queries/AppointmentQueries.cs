using System.Data.Entity;
using Patients.Application.Features.AppointmentProjections.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.AppointmentProjections.Queries;

public sealed class AppointmentQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentHistoryDto> GetAppointmentsByPatient(
        string patientId,
        [Service] PatientsReadDbContext db)
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

    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentHistoryDto> GetAppointmentsByDoctor(
        string doctorId,
        [Service] PatientsReadDbContext db)
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

    public IQueryable<AppointmentHistoryDto> GetAppointmentById(
        string appointmentId,
        [Service] PatientsReadDbContext db)
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
