namespace Users.Domain.Auth.Options;

public sealed class KeyCloakOptions
{
    public string AdminUrl { get; set; }

    public string TokenUrl { get; set; }

    public string ConfidentialClientId { get; set; }

    public string ConfidentialClientSecret { get; set; }

    public string PublicClientId { get; set; }
}
