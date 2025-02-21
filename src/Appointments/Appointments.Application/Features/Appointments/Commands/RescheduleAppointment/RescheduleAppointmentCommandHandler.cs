using Appointments.Application.Features.Commands.Appointments.Shared;
using Appointments.Application.Features.Helpers.Abstractions;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler : ICommandHandler<RescheduleAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IJwtParser _jwtParser;
	private readonly IAppointmentCommandHandlerHelper _helper;

	public RescheduleAppointmentCommandHandler(IRepositoryManager repositoryManager, IJwtParser jwtParser, IAppointmentCommandHandlerHelper helper)
	{
		_repositoryManager = repositoryManager;
		_jwtParser = jwtParser;
		_helper = helper;
	}

	public async Task<Result> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
	{
		var detailedAppointmentRes = await _repositoryManager.Appointment.GetAppointmentWithUserDetailsAsync(request.AppointmentID);

		if (detailedAppointmentRes.IsFailure)
			return Result.Failure(detailedAppointmentRes.Response);

		AppointmentWithDetailsDTO appointmentWithDetails = detailedAppointmentRes.Value!;

		var userIdRes = _jwtParser.GetIdFromToken();

		if (userIdRes.IsFailure)
			return Result.Failure(userIdRes.Response);
		if (userIdRes.Value != appointmentWithDetails.PatientId && userIdRes.Value != appointmentWithDetails.DoctorId)
			return Result.Failure(Responses.CannotRescheduleOthersAppointment);

		var helperResult = await _helper.CreateAppointment(
			appointmentWithDetails.DoctorEmail,
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
