using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Enums;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Features.Commands.Appointments.CompleteAppointments;

public sealed class CompleteAppointmentsCommandHandler : ICommandHandler<CompleteAppointmentsCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IUnitOfWork _unitOfWork;

	public CompleteAppointmentsCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork)
	{
		_repositoryManager = repositoryManager;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(CompleteAppointmentsCommand request, CancellationToken cancellationToken)
	{
		var now = DateTime.UtcNow;

		var appointmentsToCompleteRes = await _repositoryManager.Appointment
			.GetAppointmentsToCompleteAsync(now);
		if (appointmentsToCompleteRes.IsFailure)
			return Result.Failure(appointmentsToCompleteRes.Response);

		if (appointmentsToCompleteRes.Value!.Count == 0)
			return Result.Success();

		var appointmentsToComplete = appointmentsToCompleteRes.Value;

		foreach (var appointment in appointmentsToComplete)
		{
			appointment.Status = AppointmentStatus.Completed;
		}

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
