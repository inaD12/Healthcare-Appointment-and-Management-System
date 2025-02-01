using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Results;

namespace Appointments.Application.Appoints.Commands.Shared
{
	public class AppointmentCommandHandlerHelper : IAppointmentCommandHandlerHelper
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IFactoryManager _factoryManager;
		public AppointmentCommandHandlerHelper(IRepositoryManager repositoryManager, IFactoryManager factoryManager)
		{
			_repositoryManager = repositoryManager;
			_factoryManager = factoryManager;
		}

		public async Task<Result> CreateAppointment(string doctorId, string patientId, DateTime startTime, AppointmentDuration duration)
		{
			DateTime EndTime = startTime.AddMinutes(((int)duration));

			var isTimeSlotAvailableRes = await _repositoryManager.Appointment.IsTimeSlotAvailableAsync(
						doctorId,
						startTime,
						EndTime);

			bool isTimeSlotAvailable = isTimeSlotAvailableRes.Value;

			if (!isTimeSlotAvailable)
				return Result.Failure(Responses.TimeSlotNotAvailable);

			var appointment = _factoryManager.Appointment.Create(
				patientId,
				doctorId,
				startTime,
				EndTime);

			await _repositoryManager.Appointment.AddAsync(appointment);

			return Result.Success(Responses.AppointmentCreated);
		}
	}
}
