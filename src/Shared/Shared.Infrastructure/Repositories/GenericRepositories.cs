using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.Base;
using Shared.Domain.Responses;
using Shared.Domain.Results;

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

	//public virtual async Task<Result<IEnumerable<T>>> GetAllAsync()
	//{
	//	var entities = await Entities.ToListAsync();

	//	if (!entities.Any())
	//		return Result<IEnumerable<T>>.Failure(SharedResponses.EntityNotFound);

	//	return Result<IEnumerable<T>>.Success(entities);
	//}

	public virtual async Task<Result<T>> GetByIdAsync(string id)
	{
		var res = await Entities.FindAsync(id);

		if (res == null)
			return Result<T>.Failure(SharedResponses.EntityNotFound);

		return Result<T>.Success(res);
	}

	public virtual void Update(T entity)
	{
		Entities.Update(entity);
	}
}
