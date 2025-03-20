using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Responses;
using Microsoft.IdentityModel.Tokens;
using Shared.Application.Abstractions;
using Shared.Application.Helpers.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Responses;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler : ICommandHandler<RescheduleAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IJwtParser _jwtParser;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;
	public RescheduleAppointmentCommandHandler(IJwtParser jwtParser, IHAMSMapper mapper, IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository, IDateTimeProvider dateTimeProvider)
	{
		_jwtParser = jwtParser;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_appointmentRepository = appointmentRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<Result<AppointmentCommandViewModel>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
	{
		var detailedAppointment = await _appointmentRepository.GetAppointmentWithUserDetailsAsync(request.AppointmentID);

		if (detailedAppointment == null)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.AppointmentNotFound);

		var userId = _jwtParser.GetIdFromToken();

		if (userId.IsNullOrEmpty())
			return Result<AppointmentCommandViewModel>.Failure(SharedResponses.JWTNotFound);
		if (userId != detailedAppointment.PatientId && userId != detailedAppointment.DoctorId)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.CannotRescheduleOthersAppointment);

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
