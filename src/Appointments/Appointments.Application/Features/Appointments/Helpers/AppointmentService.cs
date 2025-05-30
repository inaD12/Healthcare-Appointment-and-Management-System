using Appointments.Application.Features.Appointments.Helpers.Abstractions;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Helpers;

public class AppointmentService : IAppointmentService
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IHAMSMapper _mapper;
	public AppointmentService(IRepositoryManager repositoryManager, IHAMSMapper mapper)
	{
		_repositoryManager = repositoryManager;
		_mapper = mapper;
	}

	public async Task<Result> CreateAppointment(CreateAppointmentModel model)
	{
		DateTime EndTime = model.StartTime.AddMinutes((int)model.Duration).ToUniversalTime();

		var isTimeSlotAvailableRes = await _repositoryManager.Appointment.IsTimeSlotAvailableAsync(
					model.DoctorId,
					model.StartTime,
					EndTime);

		bool isTimeSlotAvailable = isTimeSlotAvailableRes.Value;

		if (!isTimeSlotAvailable)
			return Result.Failure(Responses.TimeSlotNotAvailable);

		var appointment = _mapper.Map<Appointment>(model);

		await _repositoryManager.Appointment.AddAsync(appointment);

		return Result.Success(Responses.AppointmentCreated);
	}
}
