using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.Base;

namespace Shared.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
	private readonly DbContext _context;
	private DbSet<T> Entities => _context.Set<T>();

	public GenericRepository(DbContext context)
	{
		_context = context;
	}

	public virtual async Task AddAsync(T entity)
	{
		await Entities.AddAsync(entity);
	}

	public virtual void Delete(T entity)
	{
		Entities.Remove(entity);
	}

	public virtual async Task<T?> GetByIdAsync(string id)
	{
		var res = await Entities.FindAsync(id);

		return res;
	}

	public virtual void Update(T entity)
	{
		Entities.Update(entity);
	}
}
