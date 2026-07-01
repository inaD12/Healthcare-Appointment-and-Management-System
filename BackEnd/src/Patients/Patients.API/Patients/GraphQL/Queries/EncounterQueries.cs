using Microsoft.EntityFrameworkCore;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Authentication;

namespace Patients.API.Patients.GraphQL.Queries;

public sealed class EncounterQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encounter> GetMyEncounters(
        HttpContext httpContext,
        PatientsQueryDbContext queryDbContext)
    {
        var userId = httpContext.User.GetUserId();
        var res = queryDbContext.Encounters
            .AsNoTracking()
            .Where(e => e.PatientId == userId);

        return res;
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encounter> GetEncountersByDoctor(
        string doctorId,
        PatientsQueryDbContext queryDbContext)
    {
        var res = queryDbContext.Encounters
            .AsNoTracking()
            .Where(e => e.DoctorId == doctorId);

        return res;
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encounter> GetEncounters(
        PatientsQueryDbContext queryDbContext)
    {
        var res = queryDbContext.Encounters
            .AsNoTracking();

        return res;
    }
};