namespace Appointments.API.Appointments.Models.Responses;

public sealed record BookingQueryResponse(
	DateTime Start,
	DateTime End);

