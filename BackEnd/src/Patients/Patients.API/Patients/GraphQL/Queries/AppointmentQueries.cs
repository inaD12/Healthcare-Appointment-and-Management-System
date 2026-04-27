using System.Data.Entity;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Authentication;

namespace Patients.API.Patients.GraphQL.Queries;

public sealed class AppointmentQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentProjection> GetMyAppointments(
        HttpContext httpContext,
        PatientsQueryDbContext queryDbContext)
    {
        var userId = httpContext.User.GetUserId();
        var res = queryDbContext.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.PatientId == userId);

        return res;
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentProjection> GetAppointmentsByUserId(
        string userId,
        PatientsQueryDbContext queryDbContext)
    {
        var res = queryDbContext.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.PatientId == userId);

        return res;
    }

    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentProjection> GetAppointmentsByDoctor(
        string doctorId,
        PatientsQueryDbContext queryDbContext)
    {
        var res = queryDbContext.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.DoctorId == doctorId);
        
        return res;
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppointmentProjection> GetAppointments(
        PatientsQueryDbContext queryDbContext)
    {
        var res = queryDbContext.AppointmentProjections
            .AsNoTracking();
        
        return res;
    }

    [UseProjection]
    public IQueryable<AppointmentProjection> GetAppointmentById(
        string appointmentId,
        PatientsQueryDbContext queryDbContext)
    {
        var res = queryDbContext.AppointmentProjections
            .AsNoTracking()
            .Where(e => e.Id == appointmentId);

        return res;
    }
}