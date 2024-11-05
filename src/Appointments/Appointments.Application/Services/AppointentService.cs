using Appointments.Application.Factories;
using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Result;
using Appointments.Infrastructure.Repositories;
using Serilog;

namespace Appointments.Application.Services
{
	internal class AppointentService
	{
		private readonly IAppointmentRepository _appointmetRepository;
		private readonly IUserDataRepository _userDataRepository;
		private readonly IAppointmentFactory _appointmentFactory;

		public AppointentService(IAppointmentRepository appointmetRepository, IUserDataRepository userDataRepository)
		{
			_appointmetRepository = appointmetRepository;
			_userDataRepository = userDataRepository;
		}

		public async Task<Result> CreateAsync(CreateAppointmentDTO createAppointmentDTO)
		{
			try
			{
				var doctorDataRes = await _userDataRepository.GetUserDataByUserEmailAsync(createAppointmentDTO.DoctorEmail);
				if (doctorDataRes.IsFailure)
					return Result.Failure(Response.DoctorNotFound);

				var patientDataRes = await _userDataRepository.GetUserDataByUserEmailAsync(createAppointmentDTO.PatientEmail);
				if (patientDataRes.IsFailure)
					return Result.Failure(Response.PatientNotFound);

				var doctorData = doctorDataRes.Value;
				var patientData = patientDataRes.Value;

				var isTimeSlotAvailableRes = await _appointmetRepository.IsTimeSlotAvailableAsync(
					doctorData.UserId,
					createAppointmentDTO.ScheduledStartTime,
					createAppointmentDTO.ScheduledEndTime);

				if (isTimeSlotAvailableRes.IsFailure)
					return Result.Failure(Response.InternalError);

				bool isTimeSlotAvailable = isTimeSlotAvailableRes.Value;
				if (!isTimeSlotAvailable)
					return Result.Failure(Response.TimeSlotNotAvailable);

				var appointment = _appointmentFactory.Create(
					patientData.Id,
					doctorData.Id,
					createAppointmentDTO.ScheduledStartTime,
					createAppointmentDTO.ScheduledEndTime);

				await _appointmetRepository.AddAsync(appointment);

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
