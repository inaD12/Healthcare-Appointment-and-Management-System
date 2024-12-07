using Contracts.Results;

namespace Appointments.Domain.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task AddAsync(T entity);
		Task DeleteAsync(T entity);
		Task<Result<T>> GetByIdAsync(string id);
		Task UpdateAsync(T entity);
	}
}