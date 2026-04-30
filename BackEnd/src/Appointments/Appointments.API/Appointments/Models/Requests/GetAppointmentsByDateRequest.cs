namespace Appointments.API.Appointments.Models.Requests;

public sealed record GetAppointmentsByDateRequest(DateOnly StartDate, DateOnly EndDate);

