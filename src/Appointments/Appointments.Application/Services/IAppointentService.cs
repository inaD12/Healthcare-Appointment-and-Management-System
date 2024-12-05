using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Result;

namespace Appointments.Application.Services
{
	public interface IAppointentService
	{
		Task<Result> CreateAsync(CreateAppointmentDTO createAppointmentDTO);
		Task<Result> CancelAppointmentAsync(string appointmentId);
		Task<Result> RescheduleAppointment(RescheduleAppointmentDTO rescheduleAppointmentDTO);
	}
}