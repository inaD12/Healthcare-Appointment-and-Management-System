using Appointments.Domain.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Appointments.Commands.CompleteAppointments;

public sealed class CompleteAppointmentsCommandHandler(
	IUnitOfWork unitOfWork,
	IDateTimeProvider dateTimeProvider,
	IAppointmentRepository appointmentRepository)
	: ICommandHandler<CompleteAppointmentsCommand>
{
	public async Task<Result> Handle(CompleteAppointmentsCommand request, CancellationToken cancellationToken)
	{
		var appointmentsToComplete = await appointmentRepository
			.GetAppointmentsToCompleteAsync(dateTimeProvider.UtcNow, cancellationToken);

		if (appointmentsToComplete == null || appointmentsToComplete.Count == 0)
			return Result.Success();

		foreach (var appointment in appointmentsToComplete)
		{
			appointment.Complete();
		}

		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
