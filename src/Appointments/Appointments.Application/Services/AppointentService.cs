using Appointments.Domain.DTOS.Request;
using Appointments.Infrastructure.Repositories;

namespace Appointments.Application.Services
{
	internal class AppointentService
	{
		private readonly IAppointmentRepository _appointmetRepository;

		public AppointentService(IAppointmentRepository appointmetRepository)
		{
			_appointmetRepository = appointmetRepository;
		}

		public async Task CreateAsync(CreateAppointmentDTO createAppointmentDTO)
		{

		}
	}
}
