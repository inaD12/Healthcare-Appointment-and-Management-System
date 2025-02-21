using Shared.Domain.Enums;
using Shared.Domain.Events;
using Users.Application.Features.Users.Factories.Abstractions;

namespace Users.Application.Features.Users.Factories;

public class UserCreatedEventFactory : IUserCreatedEventFactory
{
	public UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, Roles Role)
	{
		return new UserCreatedEvent(UserId, Email, Role);
	}
}
