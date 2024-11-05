using Appointments.Application.Managers.Interfaces;
using Appointments.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.Managers
{
    internal class RepositoryManager : IRepositoryManager
	{
		private readonly IServiceProvider _serviceProvider;

		public RepositoryManager(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IAppointmentRepository Appointment => _serviceProvider.GetRequiredService<IAppointmentRepository>();
		public IUserDataRepository UserData => _serviceProvider.GetRequiredService<IUserDataRepository>();
	}
}
