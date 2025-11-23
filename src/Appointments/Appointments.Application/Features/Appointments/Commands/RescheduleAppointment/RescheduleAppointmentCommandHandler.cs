using System.Security.Claims;
using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Requirements.ModifyAppointment;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler : ICommandHandler<RescheduleAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IAuthorizationService _authService;
	public RescheduleAppointmentCommandHandler(IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository, IDateTimeProvider dateTimeProvider, IAuthorizationService authService)
	{
		_unitOfWork = unitOfWork;
		_appointmentRepository = appointmentRepository;
		_dateTimeProvider = dateTimeProvider;
		_authService = authService;
	}

	public async Task<Result<AppointmentCommandViewModel>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
	{
		var existingAppointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken);
		if (existingAppointment == null)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.AppointmentNotFound);
		}

		var requirement = new ModifyAppointmentRequirement();

		var authResult = await _authService.AuthorizeAsync(ClaimsPrincipal.Current!, existingAppointment, requirement );
		if (!authResult.Succeeded)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.CannotRescheduleOthersAppointment);
		}

		var duration = DateTimeRangeFactory.FromDuration(request.ScheduledStartTime, request.Duration);

		if (!await _appointmentRepository.IsTimeSlotAvailableAsync(existingAppointment.DoctorId, duration, cancellationToken))
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);
		}

		var appointment = Appointment.Schedule(existingAppointment.PatientId, existingAppointment.DoctorId, duration);

		await _appointmentRepository.AddAsync(appointment, cancellationToken);

		var result = existingAppointment.Reschedule(_dateTimeProvider.UtcNow);
		if (result.IsFailure)
		{
			return Result<AppointmentCommandViewModel>.Failure(result.Response);
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var appointmentCommandViewModel = appointment.ToCommandViewModel();
		return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
	}
}
