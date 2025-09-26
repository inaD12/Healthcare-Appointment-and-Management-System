using Shared.Domain.Entities;

namespace Shared.Application.IntegrationEvents;

public record UserCreatedIntegrationEvent(string Id, string Email, IReadOnlyCollection<Role> Roles);
