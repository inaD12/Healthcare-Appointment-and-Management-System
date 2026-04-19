namespace Patients.Domain.Dtos;

public sealed record AllergyDto(
    string Id,
    string Substance,
    string Reaction
);