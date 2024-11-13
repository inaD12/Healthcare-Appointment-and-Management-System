using Contracts.Events;
using Users.Application.Factories.Interfaces;

namespace Users.Application.Factories
{
	public class UserCreatedEventFactory : IUserCreatedEventFactory
	{
		public UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, string Role)
		{
			return new UserCreatedEvent(UserId, Email, Role);
		}
	}
}
