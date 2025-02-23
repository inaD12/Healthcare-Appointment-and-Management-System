namespace Shared.Domain.Events;

public record EmailConfirmationRequestedEvent(string link, string email);