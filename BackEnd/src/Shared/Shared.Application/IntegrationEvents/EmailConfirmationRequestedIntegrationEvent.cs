namespace Shared.Application.IntegrationEvents;

public record EmailConfirmationRequestedIntegrationEvent(string Link, string Email);