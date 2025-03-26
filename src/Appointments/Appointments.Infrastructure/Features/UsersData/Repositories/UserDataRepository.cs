using Appointments.Domain.Entities;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Features.UsersData.Repositories;

internal class UserDataRepository : GenericRepository<UserData>, IUserDataRepository
{
	private readonly AppointmentsDBContext _dbContext;
	public UserDataRepository(AppointmentsDBContext context) : base(context)
	{
		_dbContext = context;
	}

	public async Task<UserData?> GetUserDataByEmailAsync(string email)
	{
		var user = await _dbContext.UserData
			.FirstOrDefaultAsync(u => u.Email == email);

		return user!;
	}
}
