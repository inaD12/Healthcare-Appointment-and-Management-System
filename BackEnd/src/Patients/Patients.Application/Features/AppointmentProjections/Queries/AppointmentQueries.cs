using Microsoft.AspNetCore.Http;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;
using Shared.Infrastructure.Authentication;

namespace Patients.Application.Features.AppointmentProjections.Queries;

public sealed class AppointmentQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentHistoryDto> GetMyAppointments(
        HttpContext httpContext,
        [Service] IAppointmentReadRepository repo)
    {
        var userId = httpContext.User.GetUserId();
        return repo.GetByPatient(userId);
    }

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