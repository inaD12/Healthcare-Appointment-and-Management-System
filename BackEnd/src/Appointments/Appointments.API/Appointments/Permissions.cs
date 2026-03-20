namespace Appointments.API.Appointments;

internal static class Permissions
{
    internal const string CreateAppointment = "appointment:create";
    internal const string CancelAppointment = "appointment:cancel";
    internal const string RescheduleAppointment = "appointment:reschedule";
    internal const string GetAppointment = "appointment:read";
    internal const string GetBookings = "bookings:read";
}