using Shared.Domain.Utilities;

namespace Appointments.Domain.Utilities;

public class AppointmentsTestUtilities : SharedTestUtilities
{
	public static readonly string ValidId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string InvalidId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string WrongIdFromTokenId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string JWTExtractorInternalErrorId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string ChangeStatusInternalErrorId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string HelperInternalErrorId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string UnauthUserId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string TimeSlotUnavailableId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);

	public static readonly string PatientId = GetAverageString(AppointmentsBusinessConfiguration.PATIENTID_MIN_LENGTH, AppointmentsBusinessConfiguration.PATIENTID_MAX_LENGTH);
	public static readonly string DoctorId = GetAverageString(AppointmentsBusinessConfiguration.DOCTORID_MIN_LENGTH, AppointmentsBusinessConfiguration.DOCTORID_MAX_LENGTH);

	public static readonly string DoctorEmail = GetAverageString(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH, AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH);
	public static readonly string PatientEmail = GetAverageString(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH, AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH);
	public static readonly string InvalidEmail = GetAverageString(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH, AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH);

	public static readonly DateTime PastDate = GetDatePast();
	public static readonly DateTime CurrentDate = GetDate();
	public static readonly DateTime SoonDate = GetDateSoon();
	public static readonly DateTime FutureDate = GetDateFuture();
}
