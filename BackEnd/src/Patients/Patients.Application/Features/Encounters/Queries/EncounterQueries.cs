using Microsoft.AspNetCore.Http;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Shared.Infrastructure.Authentication;

namespace Patients.Application.Features.Encounters.Queries;

public sealed class EncounterQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Encounter> GetMyEncounters(
        HttpContext httpContext,
        [Service] IEncounterRepository repo)
    {
        var userId = httpContext.User.GetUserId();
        var res = repo.GetByPatient(userId);
        return res;
    }
};