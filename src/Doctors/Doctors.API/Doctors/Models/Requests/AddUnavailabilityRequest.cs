namespace Doctors.API.Doctors.Models.Requests;

public sealed record AddUnavailabilityRequest(
    DateTime Start,
    DateTime End,
    string Reason );