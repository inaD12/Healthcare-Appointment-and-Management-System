using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Shared.Application.Helpers.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Features.Commands.Appointments.CancelAppointment;

public sealed class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IJwtParser _jwtParser;
	private readonly IUnitOfWork _unitOfWork;
	public CancelAppointmentCommandHandler(IRepositoryManager repositoryManager, IJwtParser jwtParser, IUnitOfWork unitOfWork)
	{
		_repositoryManager = repositoryManager;
		_jwtParser = jwtParser;
		_unitOfWork = unitOfWork;
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

		_repositoryManager.Appointment.ChangeStatusAsync(appointment, AppointmentStatus.Cancelled);

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
