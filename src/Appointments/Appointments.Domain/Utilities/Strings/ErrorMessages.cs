namespace Appointments.Domain.Utilities.Strings;

public static class ErrorMessages
{
	public const string AppointmentNotFound = "Appointment not found";
	public const string DoctorNotFound = "Doctor not found";
	public const string PatientNotFound = "Patient not found";
	public const string EntityNotFound = "Entity not found";
	public const string TimeSlotNotAvailable = "Time slot is not available";
	public const string InternalError = "Internal error, please try again";
	public const string UserDataNotFound = "User data not found";
	public const string CannotCancelOthersAppointment = "You cannot cancel someone else's appointment";
	public const string CannotRescheduleOthersAppointment = "You cannot reschedule someone else's appointment";
	public const string UserIsNotADoctor = "The user you selected is not a doctor. Please choose a valid doctor.";
	public const string AppointmentNotScheduled = "Appointment is not scheduled";
	public const string AppointmentAlreadyStarted = "Appointment has already started";
	public const string NoAppointmentsFound = "No appointments found";
}
