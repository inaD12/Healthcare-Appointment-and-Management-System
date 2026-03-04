namespace Patients.API.Patients.Models.Requests;

public sealed record RemoveAllergyRequest(
    string Id,
    string AllergyId);