namespace Patients.Application.Features.Patients.Dtos;

public sealed record PatientHeaderDto(
    string Id,
    string FullName,
    DateOnly BirthDate,
    IEnumerable<string> Allergies,
    IEnumerable<string> Conditions
)
{
    public List<string> AllergiesList => Allergies.ToList();
    public List<string> ConditionsList => Conditions.ToList();
}