namespace Ratings.Domain.Utilities.Strings;

public static class ErrorMessages
{
	public const string InvalidRatingScore = "Rating score must be between 1 and 5";
	public const string AppointmentNotFound = "Appointment not found";
	public const string AppointmentNotYours = "You can't rate an appointment that is not yours";
	public const string AlreadyRated = "This appointment was already rated";
}