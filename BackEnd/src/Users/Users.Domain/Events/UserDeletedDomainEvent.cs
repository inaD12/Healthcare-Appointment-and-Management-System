using Shared.Domain.Abstractions;
using Shared.Domain.Enums;

namespace Users.Domain.Events;

public record UserDeletedDomainEvent(string Id) :IDomainEvent;
