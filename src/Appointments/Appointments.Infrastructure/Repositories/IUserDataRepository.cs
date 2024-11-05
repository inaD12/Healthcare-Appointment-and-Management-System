using Appointments.Domain.Entities;
using Appointments.Domain.Result;

namespace Appointments.Infrastructure.Repositories
{
	public interface IUserDataRepository : IGenericRepository<UserData>
	{
		Task<Result<UserData>> GetUserDataByUserEmailAsync(string id);
	}
}
