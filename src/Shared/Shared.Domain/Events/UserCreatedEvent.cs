using Shared.Domain.Enums;

namespace Shared.Domain.Events;

public record UserCreatedEvent(string Id, string Email, Roles Role);
