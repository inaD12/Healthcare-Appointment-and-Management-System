using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Enums;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Commands.Appointments.CompleteAppointments;

public sealed class CompleteAppointmentsCommandHandler : ICommandHandler<CompleteAppointmentsCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;

	public CompleteAppointmentsCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
	{
		_repositoryManager = repositoryManager;
		_unitOfWork = unitOfWork;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<Result> Handle(CompleteAppointmentsCommand request, CancellationToken cancellationToken)
	{
		var appointmentsToCompleteRes = await _repositoryManager.Appointment
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
