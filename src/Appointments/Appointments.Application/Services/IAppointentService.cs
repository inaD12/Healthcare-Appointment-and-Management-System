using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Result;

namespace Appointments.Application.Services
{
	public interface IAppointentService
	{
		Task<Result> CreateAsync(CreateAppointmentDTO createAppointmentDTO);
	}
}