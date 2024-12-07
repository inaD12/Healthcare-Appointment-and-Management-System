using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Enums;
using Appointments.Domain.UseCases;

namespace Appointments.Application.UseCases
{
	public class CompleteAppointmentsUseCase : IUseCase
	{
		private readonly IRepositoryManager _repositoryManager;

		public CompleteAppointmentsUseCase(IRepositoryManager repositoryManager)
		{
			_repositoryManager = repositoryManager;
		}

		public async Task ExecuteAsync()
		{
			var now = DateTime.UtcNow;
			var appointmentsToComplete = await _repositoryManager.Appointment.GetAppointmentsToCompleteAsync(now);

			foreach (var appointment in appointmentsToComplete)
			{
				appointment.Status = AppointmentStatus.Completed;
			}

			await _repositoryManager.Appointment.SaveChangesAsync();
		}
	}
}
