using Patients.Application.Features.Encounters.Dtos;

namespace Patients.Application.Features.Patients.Dtos;

public sealed record PatientHeaderDto(
    string Id,
    string FullName,
    DateOnly BirthDate,
    IEnumerable<AllergyDto> Allergies,
    IEnumerable<ConditionDto> Conditions
)
{
    public List<AllergyDto> AllergiesList => Allergies.ToList();
    public List<ConditionDto> ConditionsList => Conditions.ToList();
}