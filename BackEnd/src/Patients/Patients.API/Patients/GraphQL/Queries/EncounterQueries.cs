using System.Data.Entity;
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
        PatientsDbContext dbContext)
    {
        var userId = httpContext.User.GetUserId();
        var res = dbContext.Encounters
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
        PatientsDbContext dbContext)
    {
        var res = dbContext.Encounters
            .AsNoTracking()
            .Where(e => e.DoctorId == doctorId);

        return res;
    }
};