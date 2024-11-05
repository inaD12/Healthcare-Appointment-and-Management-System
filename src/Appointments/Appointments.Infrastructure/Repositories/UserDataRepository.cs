using Appointments.Domain.Entities;
using Appointments.Domain.Result;
using Appointments.Infrastructure.DBContexts;

namespace Appointments.Infrastructure.Repositories
{
	internal class UserDataRepository : GenericRepository<UserData>, IUserDataRepository
	{
		public UserDataRepository(AppointmentsDBContext context) : base(context)
		{
		}

		public Task<Result<UserData>> GetUserDataByEmailAsync(string id)
		{
			throw new NotImplementedException();
		}
	}
}
