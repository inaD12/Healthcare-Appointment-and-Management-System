using System.Security.Claims;
using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Requirements.ModifyAppointment;
using Appointments.Domain.Entities;
using Appointments.Domain.Extensions;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
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
		var detailedAppointment = await _appointmentRepository.GetAppointmentWithUserDetailsAsync(request.AppointmentId, cancellationToken);
		if (detailedAppointment == null)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.AppointmentNotFound);
		}

		var requirement = new ModifyAppointmentRequirement();

		var authResult = await _authService.AuthorizeAsync(ClaimsPrincipal.Current!, detailedAppointment.Appointment, requirement );
		if (!authResult.Succeeded)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.CannotRescheduleOthersAppointment);
		}

		var duration = DateTimeRangeFactory.FromDuration(request.ScheduledStartTime, request.Duration);

		if (!await _appointmentRepository.IsTimeSlotAvailableAsync(detailedAppointment.DoctorId, duration, cancellationToken))
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);
		}

		var appointment = Appointment.Schedule(detailedAppointment.PatientId, detailedAppointment.DoctorId, duration);

		await _appointmentRepository.AddAsync(appointment);

		var result = detailedAppointment.Appointment.Reschedule(_dateTimeProvider.UtcNow);
		if (result.IsFailure)
		{
			return Result<AppointmentCommandViewModel>.Failure(result.Response);
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var appointmentCommandViewModel = appointment.ToCommandViewModel();
		return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
	}
}
