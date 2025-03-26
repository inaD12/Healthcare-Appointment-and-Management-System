using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler : ICommandHandler<RescheduleAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;
	public RescheduleAppointmentCommandHandler(IHAMSMapper mapper, IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository, IDateTimeProvider dateTimeProvider)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_appointmentRepository = appointmentRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<Result<AppointmentCommandViewModel>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
	{
		var detailedAppointment = await _appointmentRepository.GetAppointmentWithUserDetailsAsync(request.AppointmentId);

		if (detailedAppointment == null)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.AppointmentNotFound);

		if (request.UserId != detailedAppointment.PatientId &&
			request.UserId != detailedAppointment.DoctorId &&
			!request.IsAdmin)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.CannotRescheduleOthersAppointment);
		}

		var duration = DateTimeRange.Create(request.ScheduledStartTime, request.Duration);

		if (!await _appointmentRepository.IsTimeSlotAvailableAsync(detailedAppointment.DoctorId, duration, cancellationToken))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);

		var appointment = Appointment.Schedule(detailedAppointment.PatientId, detailedAppointment.DoctorId, duration);

		await _appointmentRepository.AddAsync(appointment);

		var result = detailedAppointment.Appointment.Reschedule(_dateTimeProvider.UtcNow);
		if (result.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(result.Response);

		await _unitOfWork.SaveChangesAsync();

		var appointmentCommandViewModel = _mapper.Map<AppointmentCommandViewModel>(appointment);
		return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
	}
}
