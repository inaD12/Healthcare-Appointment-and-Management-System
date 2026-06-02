using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Domain.Entities.Base;

namespace Shared.Infrastructure.Repositories;


public abstract class GenericFactoryRepository<TContext, T>(IDbContextFactory<TContext> factory): IGenericReadRepository<T>
	where TContext : DbContext
	where T : BaseEntity
{
	protected TContext CreateDbContext()
		=> factory.CreateDbContext();
	
	public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		await using var context = CreateDbContext();

		return await context.Set<T>()
			.FindAsync([id], cancellationToken);
	}
}
