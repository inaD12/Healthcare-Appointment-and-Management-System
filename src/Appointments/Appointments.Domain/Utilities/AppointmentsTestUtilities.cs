using Contracts.Utilities;

namespace Appointments.Domain.Utilities;

public class AppointmentsTestUtilities : SharedTestUtilities
{
	public static readonly string ValidId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string InvalidId = GetAverageString(AppointmentsBusinessConfiguration.ID_MAX_LENGTH, AppointmentsBusinessConfiguration.ID_MIN_LENGTH);

	public static readonly string PatientId = GetAverageString(AppointmentsBusinessConfiguration.PATIENTID_MIN_LENGTH, AppointmentsBusinessConfiguration.PATIENTID_MAX_LENGTH);
	public static readonly string DoctorId = GetAverageString(AppointmentsBusinessConfiguration.DOCTORID_MIN_LENGTH, AppointmentsBusinessConfiguration.DOCTORID_MAX_LENGTH);
}
