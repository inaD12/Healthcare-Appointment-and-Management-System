using Shared.Domain.Enums;
using Shared.Domain.Events;
using Users.Application.Factories.Interfaces;

namespace Users.Application.Factories
{
	public class UserCreatedEventFactory : IUserCreatedEventFactory
	{
		public UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, Roles Role)
		{
			return new UserCreatedEvent(UserId, Email, Role);
		}
	}
}
