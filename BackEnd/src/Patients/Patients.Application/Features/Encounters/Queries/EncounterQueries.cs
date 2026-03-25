using Microsoft.AspNetCore.Http;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;
using Shared.Infrastructure.Authentication;

namespace Patients.Application.Features.Encounters.Queries;

public sealed class EncounterQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncounterListItemDto> GetMyEncounters(
        HttpContext httpContext,
        [Service] IEncounterRepository repo)
    {
        var userId = httpContext.User.GetUserId();
        return repo.GetByPatient(userId);
    }

    public IQueryable<EncounterDetailsDto> GetEncounterDetails(
        string encounterId,
        [Service] IEncounterRepository repo)
        => repo.GetDetails(encounterId);
}