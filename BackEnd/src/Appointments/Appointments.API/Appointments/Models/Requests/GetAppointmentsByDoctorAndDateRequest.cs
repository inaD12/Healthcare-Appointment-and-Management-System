namespace Appointments.API.Appointments.Models.Requests;

public sealed record GetAppointmentsByDoctorAndDateRequest(DateOnly StartDate, DateOnly EndDate);

