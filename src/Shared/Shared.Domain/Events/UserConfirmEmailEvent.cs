namespace Shared.Domain.Events;

public record UserConfirmEmailEvent(string link, string email);