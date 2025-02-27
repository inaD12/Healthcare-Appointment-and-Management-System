using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Abstractions;

namespace Shared.Infrastructure;

internal class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
	private readonly TContext _dbContext;

	public UnitOfWork(TContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}