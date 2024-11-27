using Contracts.Enums;

namespace Contracts.Events
{
	public record UserCreatedEvent(string UserId, string Email, Roles Role);
}
