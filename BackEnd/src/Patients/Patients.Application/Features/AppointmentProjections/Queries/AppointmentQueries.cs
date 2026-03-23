using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.AppointmentProjections.Queries;

public sealed class AppointmentQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentHistoryDto> GetAppointmentsByPatient(
        string patientId,
        [Service] IAppointmentReadRepository repo)
        => repo.GetByPatient(patientId);

    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentHistoryDto> GetAppointmentsByDoctor(
        string doctorId,
        [Service] IAppointmentReadRepository repo)
        => repo.GetByDoctor(doctorId);

    public IQueryable<AppointmentHistoryDto> GetAppointmentById(
        string appointmentId,
        [Service] IAppointmentReadRepository repo)
        => repo.GetById(appointmentId);
}