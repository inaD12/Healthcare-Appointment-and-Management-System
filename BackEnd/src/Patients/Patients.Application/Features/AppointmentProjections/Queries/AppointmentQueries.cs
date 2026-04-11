using Microsoft.AspNetCore.Http;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Shared.Infrastructure.Authentication;

namespace Patients.Application.Features.AppointmentProjections.Queries;

public sealed class AppointmentQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentProjection> GetMyAppointments(
        HttpContext httpContext,
        [Service] IAppointmentReadRepository repo)
    {
        var userId = httpContext.User.GetUserId();
        return repo.GetByPatient(userId);
    }

    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentProjection> GetAppointmentsByDoctor(
        string doctorId,
        [Service] IAppointmentReadRepository repo)
        => repo.GetByDoctor(doctorId);

    [UseProjection]
    public IQueryable<AppointmentProjection> GetAppointmentById(
        string appointmentId,
        [Service] IAppointmentReadRepository repo)
        => repo.GetById(appointmentId);
}