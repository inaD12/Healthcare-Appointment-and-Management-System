using Appointments.Domain.Entities.Base;
using Appointments.Domain.Result;
using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories
{
	internal abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly AppointmentsDBContext _context;
		private DbSet<T> Entities => _context.Set<T>();

		public GenericRepository(AppointmentsDBContext context)
		{
			_context = context;
		}

		public async Task AddAsync(T entity)
		{
			await Entities.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			Entities.Remove(entity);
			await _context.SaveChangesAsync();
		}

		//public virtual async Task<List<T>> GetAllAsync()
		//{
		//	return await Entities.ToListAsync();
		//}

		public virtual async Task<Result<T>> GetByIdAsync(int id)
		{
			var res = await Entities.FindAsync(id);

			if (res == null)
			{
				return Result<T>.Failure(Response.EntityNotFound);
			}

			return Result<T>.Success(res);
		}

		public async Task UpdateAsync(T entity)
		{
			Entities.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
