namespace Shared.Domain.Abstractions;

public interface IGenericRepository<T> where T : class
{
	Task AddAsync(T entity, CancellationToken cancellationToken = default);
	void Delete(T entity);
	Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
	void Update(T entity);
}