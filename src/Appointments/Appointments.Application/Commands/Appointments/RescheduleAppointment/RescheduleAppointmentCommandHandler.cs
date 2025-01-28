using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Contracts.Abstractions.Messaging;
using Contracts.Results;
using Appointments.Application.Managers.Interfaces;
using Appointments.Application.Helpers;
using Appointments.Domain.Responses;
using Appointments.Application.Appoints.Commands.Shared;

namespace Appointments.Application.Appoints.Commands.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler : ICommandHandler<RescheduleAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IJWTUserExtractor _jwtUserExtractor;
	private readonly IAppointmentCommandHandlerHelper _helper;

	public RescheduleAppointmentCommandHandler(IRepositoryManager repositoryManager, IJWTUserExtractor jwtUserExtractor, IAppointmentCommandHandlerHelper helper)
	{
		_repositoryManager = repositoryManager;
		_jwtUserExtractor = jwtUserExtractor;
		_helper = helper;
	}

	public async Task<Result> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
	{
		var detailedAppointmentRes = await _repositoryManager.Appointment.GetAppointmentWithUserDetailsAsync(request.AppointmentID);

		if (detailedAppointmentRes.IsFailure)
			return Result.Failure(detailedAppointmentRes.Response);

		AppointmentWithDetailsDTO appointmentWithDetails = detailedAppointmentRes.Value;

		var userIdRes = await _jwtUserExtractor.GetUserIdFromTokenAsync();
		if (userIdRes.IsFailure)
			return Result.Failure(userIdRes.Response);

		if (userIdRes.Value != appointmentWithDetails.PatientId && userIdRes.Value != appointmentWithDetails.DoctorId)
			return Result.Failure(Responses.CannotRescheduleOthersAppointment);

		var helperResult = await _helper.CreateAppointment
			(appointmentWithDetails.DoctorEmail,
			appointmentWithDetails.PatientEmail,
			request.ScheduledStartTime.ToUniversalTime(),
			request.Duration);

		if (helperResult.IsFailure)
			return Result.Failure(helperResult.Response);

		Appointment appointment = appointmentWithDetails.Appointment;

		var changeStatusRes = await _repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Rescheduled);

		return changeStatusRes;
	}
}
