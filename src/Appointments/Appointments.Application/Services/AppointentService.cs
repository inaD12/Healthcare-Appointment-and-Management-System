using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.DTOS;
using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Results;
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
					return Result.Failure(Responses.DoctorNotFound);

				var patientDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(createAppointmentDTO.PatientEmail);
				if (patientDataRes.IsFailure)
					return Result.Failure(Responses.PatientNotFound);

				var doctorData = doctorDataRes.Value;
				var patientData = patientDataRes.Value;

				DateTime EndTime = createAppointmentDTO.ScheduledStartTime.AddMinutes(((int)createAppointmentDTO.Duration));

				var isTimeSlotAvailableRes = await _repositoryManager.Appointment.IsTimeSlotAvailableAsync(
					doctorData.UserId,
					createAppointmentDTO.ScheduledStartTime,
					EndTime);

				if (isTimeSlotAvailableRes.IsFailure)
					return Result.Failure(Responses.InternalError);

				bool isTimeSlotAvailable = isTimeSlotAvailableRes.Value;
				if (!isTimeSlotAvailable)
					return Result.Failure(Responses.TimeSlotNotAvailable);

				var appointment = _factoryManager.Appointment.Create(
					patientData.UserId,
					doctorData.UserId,
					createAppointmentDTO.ScheduledStartTime,
					EndTime);

				await _repositoryManager.Appointment.AddAsync(appointment);

				return Result.Success(Responses.AppointmentCreated);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CreateAsync() in AppointentService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Responses.InternalError);
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

				var userIdRes = await GetUserIdFromTokenAsync();
				if (userIdRes.IsFailure)
					return Result.Failure(userIdRes.Response);

				if (!IsUserAuthorized(userIdRes.Value, appointment.PatientId, appointment.DoctorId))
					return Result.Failure(Responses.CannotCancelOthersAppointment);

				var changeStatusRes = await _repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Cancelled);

				return changeStatusRes;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CancelAppointmentAsync() in AppointentService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Responses.InternalError);
			}
		}

		public async Task<Result> RescheduleAppointment(RescheduleAppointmentDTO rescheduleAppointmentDTO)
		{
			try
			{
				var detailedAppointmentRes = await _repositoryManager.Appointment.GetAppointmentWithUserDetailsAsync(rescheduleAppointmentDTO.AppointmentID);

				if (detailedAppointmentRes.IsFailure)
					return Result.Failure(detailedAppointmentRes.Response);

				AppointmentWithDetailsDTO appointmentWithDetails = detailedAppointmentRes.Value;

				//var userIdRes = await GetUserIdFromTokenAsync();
				//if (userIdRes.IsFailure)
				//	return Result.Failure(userIdRes.Response);

				//if (!IsUserAuthorized(userIdRes.Value, appointmentWithDetails.PatientId, appointmentWithDetails.DoctorId))
				//	return Result.Failure(Response.CannotRescheduleOthersAppointment);

				var createRes = await CreateAsync(_factoryManager.CreateAppointmentDTO.Create(
					appointmentWithDetails.PatientEmail,
					appointmentWithDetails.DoctorEmail,
					rescheduleAppointmentDTO.ScheduledStartTime,
					rescheduleAppointmentDTO.Duration));

				if (createRes.IsFailure)
					return Result.Failure(createRes.Response);

				var appointmentRes = await _repositoryManager.Appointment.GetByIdAsync(rescheduleAppointmentDTO.AppointmentID);

				if (appointmentRes.IsFailure)
					return Result.Failure(appointmentRes.Response);

				Appointment appointment = appointmentRes.Value;

				var changeStatusRes = await _repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Rescheduled);

				return changeStatusRes;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in RescheduleAppointment() in AppointentService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Responses.InternalError);
			}
		}
		private async Task<Result<string>> GetUserIdFromTokenAsync()
		{
			try
			{
				var userIdRes = _jwtParser.GetIdFromToken();
				if (userIdRes.IsFailure)
					return Result<string>.Failure(userIdRes.Response);

				return Result<string>.Success(userIdRes.Value);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserIdFromTokenAsync(): {ex.Message}");
				return Result<string>.Failure(Responses.InternalError);
			}
		}

		private bool IsUserAuthorized(string userId, string patientId, string doctorId)
		{
			return userId == patientId || userId == doctorId;
		}
	}
}
