namespace Appointments.API.Appointments.Models.Requests;

public sealed record GetBookingsByDoctorAndDateRequest(DateOnly StartDate, DateOnly EndDate);

