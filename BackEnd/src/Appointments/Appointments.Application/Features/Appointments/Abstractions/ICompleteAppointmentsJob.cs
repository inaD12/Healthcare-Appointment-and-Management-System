namespace Appointments.Application.Features.Appointments.Abstractions;

public interface ICompleteAppointmentsJob
{
	Task Execute(CancellationToken cancellationToken);
}