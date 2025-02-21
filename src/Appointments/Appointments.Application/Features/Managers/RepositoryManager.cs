using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Abstractions.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.Features.Jobs.Managers;

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
