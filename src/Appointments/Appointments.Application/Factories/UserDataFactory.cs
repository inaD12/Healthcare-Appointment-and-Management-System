using Appointments.Domain.Entities;

namespace Appointments.Application.Factories
{
	public class UserDataFactory : IUserDataFactory
	{
		public UserData Create(string userId, string email, string role)
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
