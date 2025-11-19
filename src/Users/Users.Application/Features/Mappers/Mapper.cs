using Shared.Application.IntegrationEvents;
using Users.Domain.Events;

namespace Users.Application.Features.Mappers;

public static class Mapper
{
    public static UserCreatedIntegrationEvent ToIntEvent(
        this UserCreatedDomainEvent domainEvent)
        => new(
            domainEvent.Id,
            domainEvent.Email,
            domainEvent.Roles);
}
  