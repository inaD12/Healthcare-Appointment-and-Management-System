using Shared.Application.IntegrationEvents;
using Users.Domain.Events;

namespace Users.Infrastructure.Features.Mappers;

public static class EventMapper
{
    public static UserUpdatedNamesIntegrationEvent ToIntegrationEvent(
        this UserUpdatedNamesDomainEvent domainEvent)
        => new(
            domainEvent.Id,
            domainEvent.FirstName,
            domainEvent.LastName);
    
    public static UserCreatedIntegrationEvent ToIntEvent(
        this UserCreatedDomainEvent domainEvent)
        => new(
            domainEvent.Id,
            domainEvent.Email,
            domainEvent.FirstName,
            domainEvent.LastName,
            domainEvent.BirthDay,
            domainEvent.Roles);
}