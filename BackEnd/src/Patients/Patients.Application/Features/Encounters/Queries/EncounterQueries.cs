using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries;

public sealed class EncounterQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncounterListItemDto> GetEncountersByPatient(
        string patientId,
        [Service] IEncounterRepository repo)
        => repo.GetByPatient(patientId);

    public IQueryable<EncounterDetailsDto> GetEncounterDetails(
        string encounterId,
        [Service] IEncounterRepository repo)
        => repo.GetDetails(encounterId);
}