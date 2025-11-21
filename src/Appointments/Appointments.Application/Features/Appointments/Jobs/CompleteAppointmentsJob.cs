using Appointments.Application.Features.Commands.Appointments.CompleteAppointments;
using Appointments.Domain.Infrastructure.Abstractions;
using MediatR;

namespace Appointments.Application.Features.Jobs;

internal class CompleteAppointmentsJob : ICompleteAppointmentsJob
{
	private readonly ISender _sender;

	public CompleteAppointmentsJob(ISender sender)
	{
		_sender = sender;
	}

	public async Task Execute(CancellationToken cancellationToken)
	{
		var command = new CompleteAppointmentsCommand();

		await _sender.Send(command, cancellationToken);
	}
}
