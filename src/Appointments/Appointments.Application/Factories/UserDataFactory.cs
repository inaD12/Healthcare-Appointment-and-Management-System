﻿using Appointments.Domain.Entities;
using Shared.Domain.Enums;

namespace Appointments.Application.Factories;

public class UserDataFactory : IUserDataFactory
{
	public UserData Create(string userId, string email, Roles role)
	{
		return new UserData
		{
			Id = Guid.NewGuid().ToString(),
			Email = email,
			Role = role,
			UserId = userId,
		};
	}
}
