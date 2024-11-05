﻿using Appointments.Domain.Entities;
using Appointments.Domain.Result;
using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Appointments.Infrastructure.Repositories
{
	internal class UserDataRepository : GenericRepository<UserData>, IUserDataRepository
	{
		private readonly UserDataDBContext _dbContext;
		public UserDataRepository(UserDataDBContext context) : base(context)
		{
			_dbContext = context;
		}

		public async Task<Result<UserData>> GetUserDataByEmailAsync(string email)
		{
			try
			{
				var user = await _dbContext.UserData
					.FirstOrDefaultAsync(u => u.Email == email);

				if (user == null)
					return Result<UserData>.Failure(Response.UserDataNotFound);

				return Result<UserData>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserDataByEmailAsync() in UserDataRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result<UserData>.Failure(Response.InternalError);
			}
		}
	}
}
