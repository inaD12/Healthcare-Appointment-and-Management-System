namespace Shared.Application.IntegrationEvents;

public record EmailConfirmationRequestedIntegrationEvent(string link, string email);