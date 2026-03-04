namespace Users.Domain.Auth.Options;

public sealed class KeyCloakOptions
{
    public required string AdminUrl { get; init; }
    public required string TokenUrl { get; init; }

    public required string ConfidentialClientId { get; init; }
    public required string ConfidentialClientSecret { get; init; }

    public required string PublicClientId { get; init; }
}
