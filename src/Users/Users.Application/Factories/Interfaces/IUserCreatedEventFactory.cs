using Contracts.Enums;
using Contracts.Events;

namespace Users.Application.Factories.Interfaces
{
    public interface IUserCreatedEventFactory
    {
        UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, Roles Role);
    }
}