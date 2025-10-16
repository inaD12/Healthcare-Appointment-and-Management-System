namespace Doctors.API.Doctors.Models.Requests;

public sealed record AddExtraAvailabilityRequest(
    DateTime Start,
    DateTime End,
    string Reason );