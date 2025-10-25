namespace Shared.Application.IntegrationEvents;

public sealed record UserUpdatedNamesIntegrationEvent(
    string UserId,
    string FirstName,
    string LastName
);
