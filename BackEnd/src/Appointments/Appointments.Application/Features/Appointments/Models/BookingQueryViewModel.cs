namespace Appointments.Application.Features.Appointments.Models;

public sealed record BookingQueryViewModel(
	DateTime Start,
	DateTime End);

