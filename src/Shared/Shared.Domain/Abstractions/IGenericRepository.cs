using Shared.Domain.Results;

namespace Shared.Domain.Abstractions;

public interface IGenericRepository<T> where T : class
{
	Task AddAsync(T entity);
	void Delete(T entity);
	Task<Result<T>> GetByIdAsync(string id);
	//Task<Result<IEnumerable<T>>> GetAllAsync();
	void Update(T entity);
}