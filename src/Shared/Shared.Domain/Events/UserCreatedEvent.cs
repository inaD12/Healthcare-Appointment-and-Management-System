using Shared.Domain.Enums;

namespace Shared.Domain.Events;

public record UserCreatedEvent(string UserId, string Email, Roles Role);
