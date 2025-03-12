using Appointments.Domain.Abstractions.Repository;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Commands.Appointments.CompleteAppointments;

public sealed class CompleteAppointmentsCommandHandler : ICommandHandler<CompleteAppointmentsCommand>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;

	public CompleteAppointmentsCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IAppointmentRepository repositoryManager)
	{
		_unitOfWork = unitOfWork;
		_dateTimeProvider = dateTimeProvider;
		_appointmentRepository = repositoryManager;
	}

	public async Task<Result> Handle(CompleteAppointmentsCommand request, CancellationToken cancellationToken)
	{
		var appointmentsToCompleteRes = await _appointmentRepository
			.GetAppointmentsToCompleteAsync(_dateTimeProvider.UtcNow);
		if (appointmentsToCompleteRes.IsFailure)
			return Result.Failure(appointmentsToCompleteRes.Response);

		if (appointmentsToCompleteRes.Value!.Count == 0)
			return Result.Success();

		var appointmentsToComplete = appointmentsToCompleteRes.Value;

		foreach (var appointment in appointmentsToComplete)
		{
			var res = appointment.Complete();
		}

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
