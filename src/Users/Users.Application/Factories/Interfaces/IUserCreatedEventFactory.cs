using Shared.Domain.Enums;
using Shared.Domain.Events;

namespace Users.Application.Factories.Interfaces
{
    public interface IUserCreatedEventFactory
    {
        UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, Roles Role);
    }
}