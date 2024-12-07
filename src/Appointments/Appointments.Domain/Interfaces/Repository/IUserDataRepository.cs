using Appointments.Domain.Entities;
using Contracts.Results;

namespace Appointments.Domain.Repositories
{
	public interface IUserDataRepository : IGenericRepository<UserData>
	{
		Task<Result<UserData>> GetUserDataByEmailAsync(string id);
	}
}
