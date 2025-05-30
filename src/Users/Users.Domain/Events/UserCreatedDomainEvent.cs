using Shared.Domain.Abstractions;
using Shared.Domain.Enums;

namespace Users.Domain.Events;

public record UserCreatedDomainEvent(string Id, string Email, Roles Role) :IDomainEvent;
