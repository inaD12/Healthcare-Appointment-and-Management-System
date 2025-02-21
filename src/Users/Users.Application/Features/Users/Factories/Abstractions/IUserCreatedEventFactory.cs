using Shared.Domain.Enums;
using Shared.Domain.Events;

namespace Users.Application.Features.Users.Factories.Abstractions;

public interface IUserCreatedEventFactory
{
	UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, Roles Role);
}