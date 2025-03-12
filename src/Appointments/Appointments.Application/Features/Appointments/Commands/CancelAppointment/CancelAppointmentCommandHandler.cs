using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.Entities;
using Appointments.Domain.Responses;
using Shared.Application.Helpers.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Commands.Appointments.CancelAppointment;

public sealed class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IJwtParser _jwtParser;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;
	public CancelAppointmentCommandHandler(IJwtParser jwtParser, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IAppointmentRepository repositoryManager)
	{
		_jwtParser = jwtParser;
		_unitOfWork = unitOfWork;
		_dateTimeProvider = dateTimeProvider;
		_appointmentRepository = repositoryManager;
	}
	public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointmentRes = await _appointmentRepository.GetByIdAsync(request.AppointmentId);

		if (appointmentRes.IsFailure)
			return Result.Failure(appointmentRes.Response);

		Appointment appointment = appointmentRes.Value!;

		var userIdRes = _jwtParser.GetIdFromToken();
		if (userIdRes.IsFailure)
			return Result.Failure(userIdRes.Response);

		if (userIdRes.Value != appointment.PatientId && userIdRes.Value != appointment.DoctorId)
			return Result.Failure(ResponseList.CannotCancelOthersAppointment);

		var res = appointment.Cancel(_dateTimeProvider.UtcNow);
		if (res.IsFailure)
			return res;

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
