namespace Appointments.Domain.Infrastructure.Abstractions;

public interface ICompleteAppointmentsJob
{
	Task Execute(CancellationToken cancellationToken);
}