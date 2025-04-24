namespace Shared.Utilities;

public abstract class AppClaims
{
	public static readonly List<string> All = new() { Id, Role };

	public const string Id = nameof(Id);
	public const string Role = nameof(Role);
}
