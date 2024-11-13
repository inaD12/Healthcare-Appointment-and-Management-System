using Contracts.Events;

namespace Users.Application.Factories.Interfaces
{
    public interface IUserCreatedEventFactory
    {
        UserCreatedEvent CreateUserCreatedEvent(string UserId, string Email, string Role);
    }
}