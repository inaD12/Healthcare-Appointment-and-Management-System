using Shared.Domain.Abstractions;
using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Domain.Events;

public record UserCreatedDomainEvent(string Id, string Email, string FirstName, string LastName, DateOnly BirthDay, IReadOnlyCollection<Roles> Roles) :IDomainEvent;
