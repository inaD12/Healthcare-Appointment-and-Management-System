namespace Contracts.Events
{
	public record UserCreatedEvent(string UserId, string Email, string Role);
}
