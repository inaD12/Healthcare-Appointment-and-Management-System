using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Abstractions.Messaging;
using Contracts.Results;

namespace Appointments.Application.Appoints.Commands.CancelAppointment;

public sealed class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IJWTUserExtractor _jwtUserExtractor;
	public CancelAppointmentCommandHandler(IRepositoryManager repositoryManager, IJWTUserExtractor jwtUserExtractor)
	{
		_repositoryManager = repositoryManager;
		_jwtUserExtractor = jwtUserExtractor;
	}
	public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointmentRes = await _repositoryManager.Appointment.GetByIdAsync(request.appointmentId);

		if (appointmentRes.IsFailure)
			return Result.Failure(appointmentRes.Response);

		Appointment appointment = appointmentRes.Value;

		var userIdRes = await _jwtUserExtractor.GetUserIdFromTokenAsync();
		if (userIdRes.IsFailure)
			return Result.Failure(userIdRes.Response);

		if (userIdRes.Value != appointment.PatientId && userIdRes.Value != appointment.DoctorId)
			return Result.Failure(Responses.CannotCancelOthersAppointment);

		var changeStatusRes = await _repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Cancelled);

		return changeStatusRes;
	}
}
