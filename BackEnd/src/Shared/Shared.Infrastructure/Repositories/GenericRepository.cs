using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.Base;

namespace Shared.Infrastructure.Repositories;

public abstract class GenericRepository<T>(DbContext context) : IGenericRepository<T>
	where T : BaseEntity
{
	private DbSet<T> Entities => context.Set<T>();

	public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
	{
		await Entities.AddAsync(entity, cancellationToken);
	}

	public virtual void Delete(T entity)
	{
		Entities.Remove(entity);
	}

	public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		var res = await Entities.FindAsync(id, cancellationToken);

		return res;
	}

	public virtual void Update(T entity)
	{
		Entities.Update(entity);
	}
}
