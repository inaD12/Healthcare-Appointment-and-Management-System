using Appointments.Domain.Enums;
using Contracts.Abstractions.Messaging;

namespace Appointments.Application.Appoints.Commands.CreateAppointment
{
	public sealed record CreateAppointmentCommand(
	string PatientEmail,
	string DoctorEmail,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration) : ICommand;
}
