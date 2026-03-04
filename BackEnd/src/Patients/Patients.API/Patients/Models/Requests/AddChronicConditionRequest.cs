namespace Patients.API.Patients.Models.Requests;

public sealed record AddChronicConditionRequest(
    string Id,
    string Name);