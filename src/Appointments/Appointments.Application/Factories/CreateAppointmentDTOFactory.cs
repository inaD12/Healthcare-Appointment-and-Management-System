using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Enums;

namespace Appointments.Application.Factories
{
	public class CreateAppointmentDTOFactory : ICreateAppointmentDTOFactory
	{
		public CreateAppointmentDTO Create(string patientEmail, string doctorEmail, DateTime ScheduledStartTime, AppointmentDuration duration)
		{
			return new CreateAppointmentDTO
			{
				PatientEmail = patientEmail,
				DoctorEmail = doctorEmail,
				ScheduledStartTime = ScheduledStartTime,
				Duration = duration
			};
		}
	}
}
