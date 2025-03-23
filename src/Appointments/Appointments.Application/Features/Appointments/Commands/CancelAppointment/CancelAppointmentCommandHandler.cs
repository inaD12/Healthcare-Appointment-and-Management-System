using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Commands.Appointments.CancelAppointment;

public sealed class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;
	public CancelAppointmentCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IAppointmentRepository repositoryManager)
	{
		_unitOfWork = unitOfWork;
		_dateTimeProvider = dateTimeProvider;
		_appointmentRepository = repositoryManager;
	}
	public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);

		if (appointment == null)
			return Result.Failure(ResponseList.AppointmentNotFound);

		if (request.userId != appointment.PatientId && request.userId != appointment.DoctorId)
			return Result.Failure(ResponseList.CannotCancelOthersAppointment);

		var res = appointment.Cancel(_dateTimeProvider.UtcNow);
		if (res.IsFailure)
			return res;

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
