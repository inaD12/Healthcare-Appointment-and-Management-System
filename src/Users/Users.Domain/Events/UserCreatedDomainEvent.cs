using Shared.Domain.Abstractions;
using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Domain.Events;

public record UserCreatedDomainEvent(string Id, string Email, IReadOnlyCollection<Roles> Roles) :IDomainEvent;
