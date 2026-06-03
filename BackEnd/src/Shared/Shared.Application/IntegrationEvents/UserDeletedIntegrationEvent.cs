using Shared.Domain.Enums;

namespace Shared.Application.IntegrationEvents;

public record UserDeletedIntegrationEvent(string Id, IReadOnlyCollection<Roles> Roles);
