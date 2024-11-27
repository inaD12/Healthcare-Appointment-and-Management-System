using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Result;
using Serilog;

namespace Appointments.Application.Services
{
	internal class AppointentService : IAppointentService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IFactoryManager _factoryManager;
		private readonly IJwtParser _jwtParser;

		public AppointentService(IRepositoryManager repositoryManager, IFactoryManager factoryManager, IJwtParser jwtParser)
		{
			_repositoryManager = repositoryManager;
			_factoryManager = factoryManager;
			_jwtParser = jwtParser;
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

				return Result.Success(Response.AppointmentCreated);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CreateAsync() in AppointentService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}

		public async Task<Result> CancelAppointmentAsync(string appointmentId)
		{
			try
			{
				var appointmentRes = await _repositoryManager.Appointment.GetByIdAsync(appointmentId);

				if (appointmentRes.IsFailure)
					return Result.Failure(appointmentRes.Response);

				Appointment appointment = appointmentRes.Value;

				var userIdRes = _jwtParser.GetIdFromToken();

				if (userIdRes.IsFailure)
					return Result.Failure(userIdRes.Response);

				string userId = userIdRes.Value;

				if (userId != appointment.PatientId || userId != appointment.DoctorId)
				{
					return Result.Failure(Response.CannotCancelOthersAppointment);
				}

				var changeStatusRes = await _repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Cancelled);

				return changeStatusRes;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CancelAppointmentAsync() in AppointentService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}
	}
}
