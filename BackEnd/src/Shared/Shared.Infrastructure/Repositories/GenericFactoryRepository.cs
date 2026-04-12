using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.Base;

namespace Shared.Infrastructure.Repositories;


public abstract class GenericFactoryRepository<TContext, T>
	where TContext : DbContext
	where T : BaseEntity
{
	private readonly IDbContextFactory<TContext> _factory;

	protected GenericFactoryRepository(IDbContextFactory<TContext> factory)
	{
		_factory = factory;
	}

	protected TContext CreateDbContext()
		=> _factory.CreateDbContext();
	
	public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
	{
		await using var context = CreateDbContext();

		await context.Set<T>().AddAsync(entity, cancellationToken);
	}

	public virtual void Delete(T entity)
	{
		using var context = CreateDbContext();

		context.Set<T>().Remove(entity);
	}

	public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		await using var context = CreateDbContext();

		return await context.Set<T>()
			.FindAsync([id], cancellationToken);
	}

	public virtual void Update(T entity)
	{
		using var context = CreateDbContext();

		context.Set<T>().Update(entity);
	}
}
