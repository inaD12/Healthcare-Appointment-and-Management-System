using Appointments.Domain.Entities;
using Contracts.Enums;

namespace Appointments.Application.Factories
{
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
}
