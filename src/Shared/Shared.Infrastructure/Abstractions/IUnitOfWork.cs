namespace Shared.Infrastructure.Abstractions
{
	internal interface IUnitOfWork
	{
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}