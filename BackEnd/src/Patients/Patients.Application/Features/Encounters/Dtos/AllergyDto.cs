namespace Patients.Application.Features.Encounters.Dtos;

public sealed record AllergyDto(
    string Id,
    string Substance,
    string Reaction
);