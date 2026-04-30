using Shared.Domain.Enums;

namespace Shared.Application.IntegrationEvents;

public record UserCreatedIntegrationEvent(string Id, string Email, string FirstName, string LastName, DateOnly BirthDay, IReadOnlyCollection<Roles> Roles);
