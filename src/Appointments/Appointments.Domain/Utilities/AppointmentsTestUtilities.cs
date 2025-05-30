using Appointments.Domain.Entities.Enums;
using Shared.Domain.Enums;
using Shared.Domain.Utilities;

namespace Appointments.Domain.Utilities;

public class AppointmentsTestUtilities : SharedTestUtilities
{
	public static readonly string ValidId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string InvalidId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string UnaothorizedId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);

	public static readonly string PatientId = GetAverageString(AppointmentsBusinessConfiguration.PATIENTID_MIN_LENGTH, AppointmentsBusinessConfiguration.PATIENTID_MAX_LENGTH);
	public static readonly string DoctorId = GetAverageString(AppointmentsBusinessConfiguration.DOCTORID_MIN_LENGTH, AppointmentsBusinessConfiguration.DOCTORID_MAX_LENGTH);

	public static readonly string DoctorEmail = GetAverageString(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH, AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH) + "@gmail.com";
	public static readonly string PatientEmail = GetAverageString(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH, AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH) + "@gmail.com";
	public static readonly string InvalidEmail = GetAverageString(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH, AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH);

	public static readonly DateTime PastDate = GetDatePast();
	public static readonly DateTime CurrentDate = GetDate();
	public static readonly DateTime SoonDate = GetDateSoon();
	public static readonly DateTime FutureDate = GetDateFuture();

	public static readonly AppointmentStatus ValidAppointmentStatus = AppointmentStatus.Scheduled;

	public static readonly int ValidPageValue = 1;
	public static readonly int ValidPageSizeValue = 20;

	public static readonly string ValidSortPropertyName = "Id";
	public static readonly string InvalidSortPropertyName = "Idd";

	public static readonly SortOrder ValidSortOrderProperty = SortOrder.ASC;
}
