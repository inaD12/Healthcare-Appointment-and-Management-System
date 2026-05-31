using Appointments.Application.Features.Appointments.Abstractions;
using Appointments.Application.Features.Appointments.Commands.CompleteAppointments;
using MediatR;

namespace Appointments.Application.Features.Appointments.Jobs;

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
