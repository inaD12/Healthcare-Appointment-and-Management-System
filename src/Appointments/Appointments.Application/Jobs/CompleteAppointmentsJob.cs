using Appointments.Application.Appointments.Commands.CompleteAppointments;
using MediatR;

namespace Appointments.Application.Jobs
{
	public class CompleteAppointmentsJob
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
}
