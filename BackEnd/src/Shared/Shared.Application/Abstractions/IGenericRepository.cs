namespace Shared.Application.Abstractions;

public interface IGenericRepository<T> where T : class
{
	Task AddAsync(T entity, CancellationToken cancellationToken = default);
	void Delete(T entity);
	Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
	void Update(T entity);
}