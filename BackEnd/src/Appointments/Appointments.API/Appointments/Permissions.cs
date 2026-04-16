namespace Appointments.API.Appointments;

internal static class Permissions
{
    internal const string CreateAppointment = "appointment:create";
    internal const string CancelAppointment = "appointment:cancel";
    internal const string RescheduleAppointment = "appointment:reschedule";
    internal const string GetMyAppointment = "appointment:mine:read";
    internal const string GetBookings = "bookings:read";
    
    internal const string GetAppointment = "appointment:read";
    internal const string CreateAppointmentByAdmin = "appointment:admin:create";
    internal const string CancelAppointmentByAdmin = "appointment:admin:cancel";
    internal const string RescheduleAppointmentByAdmin = "appointment:admin:reschedule";
}