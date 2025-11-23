using Appointments.Domain.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Appointments.Commands.CompleteAppointments;

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
		var appointmentsToComplete = await _appointmentRepository
			.GetAppointmentsToCompleteAsync(_dateTimeProvider.UtcNow, cancellationToken);

		if (appointmentsToComplete == null || appointmentsToComplete.Count == 0)
			return Result.Success();

		foreach (var appointment in appointmentsToComplete!)
		{
			var res = appointment.Complete();
		}

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
