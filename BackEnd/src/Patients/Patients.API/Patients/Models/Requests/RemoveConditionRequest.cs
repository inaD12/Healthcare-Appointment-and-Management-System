namespace Patients.API.Patients.Models.Requests;

public sealed record RemoveConditionRequest(
    string Id,
    string ConditionId);