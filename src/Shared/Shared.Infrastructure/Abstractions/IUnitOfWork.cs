namespace Shared.Infrastructure.Abstractions;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}