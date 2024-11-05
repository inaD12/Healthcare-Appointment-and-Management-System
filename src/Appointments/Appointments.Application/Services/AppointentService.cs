using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Result;
using Serilog;

namespace Appointments.Application.Services
{
	internal class AppointentService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IFactoryManager _factoryManager;

		public AppointentService(IRepositoryManager repositoryManager, IFactoryManager factoryManager)
		{
			_repositoryManager = repositoryManager;
			_factoryManager = factoryManager;
		}

		public async Task<Result> CreateAsync(CreateAppointmentDTO createAppointmentDTO)
		{
			try
			{
				var doctorDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(createAppointmentDTO.DoctorEmail);
				if (doctorDataRes.IsFailure)
					return Result.Failure(Response.DoctorNotFound);

				var patientDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(createAppointmentDTO.PatientEmail);
				if (patientDataRes.IsFailure)
					return Result.Failure(Response.PatientNotFound);

				var doctorData = doctorDataRes.Value;
				var patientData = patientDataRes.Value;

				var isTimeSlotAvailableRes = await _repositoryManager.Appointment.IsTimeSlotAvailableAsync(
					doctorData.UserId,
					createAppointmentDTO.ScheduledStartTime,
					createAppointmentDTO.ScheduledEndTime);

				if (isTimeSlotAvailableRes.IsFailure)
					return Result.Failure(Response.InternalError);

				bool isTimeSlotAvailable = isTimeSlotAvailableRes.Value;
				if (!isTimeSlotAvailable)
					return Result.Failure(Response.TimeSlotNotAvailable);

				var appointment = _factoryManager.Appointment.Create(
					patientData.Id,
					doctorData.Id,
					createAppointmentDTO.ScheduledStartTime,
					createAppointmentDTO.ScheduledEndTime);

				await _repositoryManager.Appointment.AddAsync(appointment);

				return Result.Success(Response.Ok);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CreateAsync() in AppointentService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}
	}
}
