namespace Shared.Utilities;

public abstract class AppClaims
{
	public const string Id = nameof(Id);
	public const string Issuer = "iss";
	public const string Audience = "aud";
	public const string Expiration = "exp";
}
