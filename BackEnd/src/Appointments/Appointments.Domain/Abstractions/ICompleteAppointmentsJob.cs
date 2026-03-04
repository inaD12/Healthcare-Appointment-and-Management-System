namespace Appointments.Domain.Abstractions;

public interface ICompleteAppointmentsJob
{
	Task Execute(CancellationToken cancellationToken);
}