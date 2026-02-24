namespace Patients.API.Patients.Models.Requests;

public sealed record AddAllergyRequest(
    string Substance,
    string Reaction);