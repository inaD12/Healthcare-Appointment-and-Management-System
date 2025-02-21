using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.Entities;
using Appointments.Domain.Responses;
using Appointments.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Results;
using Shared.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Features.Repositories;

internal class UserDataRepository : GenericRepository<UserData>, IUserDataRepository
{
	private readonly AppointmentsDBContext _dbContext;
	public UserDataRepository(AppointmentsDBContext context) : base(context)
	{
		_dbContext = context;
	}

	public async Task<Result<UserData>> GetUserDataByEmailAsync(string email)
	{
		var user = await _dbContext.UserData
			.FirstOrDefaultAsync(u => u.Email == email);

		if (user == null)
			return Result<UserData>.Failure(Responses.UserDataNotFound);

		return Result<UserData>.Success(user);
	}
}
