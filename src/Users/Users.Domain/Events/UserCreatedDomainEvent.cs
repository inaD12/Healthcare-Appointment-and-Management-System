using Shared.Domain.Abstractions;
using Shared.Domain.Entities;

namespace Users.Domain.Events;

public record UserCreatedDomainEvent(string Id, string Email, IReadOnlyCollection<Role> Roles) :IDomainEvent;
