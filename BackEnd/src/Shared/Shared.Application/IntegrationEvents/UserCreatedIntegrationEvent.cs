using Shared.Domain.Enums;

namespace Shared.Application.IntegrationEvents;

public record UserCreatedIntegrationEvent(string Id, string Email, IReadOnlyCollection<Roles> Roles);
