using Appointments.Domain.Result;

namespace Appointments.Infrastructure.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task AddAsync(T entity);
		Task DeleteAsync(T entity);
		Task<Result<T>> GetByIdAsync(int id);
		Task UpdateAsync(T entity);
	}
}