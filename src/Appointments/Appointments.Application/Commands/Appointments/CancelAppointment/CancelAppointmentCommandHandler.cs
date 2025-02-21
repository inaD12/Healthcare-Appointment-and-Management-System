using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Commands.Appointments.CancelAppointment;

public sealed class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IJwtParser _jwtParser;
	public CancelAppointmentCommandHandler(IRepositoryManager repositoryManager, IJwtParser jwtParser)
	{
		_repositoryManager = repositoryManager;
		_jwtParser = jwtParser;
	}
	public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointmentRes = await _repositoryManager.Appointment.GetByIdAsync(request.AppointmentId);

		if (appointmentRes.IsFailure)
			return Result.Failure(appointmentRes.Response);

		Appointment appointment = appointmentRes.Value!;

		var userIdRes = _jwtParser.GetIdFromToken();
		if (userIdRes.IsFailure)
			return Result.Failure(userIdRes.Response);

		if (userIdRes.Value != appointment.PatientId && userIdRes.Value != appointment.DoctorId)
			return Result.Failure(Responses.CannotCancelOthersAppointment);

		var changeStatusRes = await _repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Cancelled);

		return changeStatusRes;
	}
}
