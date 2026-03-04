namespace Doctors.API.Doctors.Models.Requests;

public sealed record RemoveUnavailabilityRequest(
    DateTime Start,
    DateTime End );