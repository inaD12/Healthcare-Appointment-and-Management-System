namespace Doctors.API.Doctors.Models.Requests;

public sealed record RemoveExtraAvailabilityRequest(
    DateTime Start,
    DateTime End );