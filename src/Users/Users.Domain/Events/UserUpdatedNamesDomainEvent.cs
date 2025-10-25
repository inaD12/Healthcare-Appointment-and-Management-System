using Shared.Domain.Abstractions;
using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Domain.Events;

public record UserUpdatedNamesDomainEvent(string Id, string FirstName, string LastName) :IDomainEvent;
