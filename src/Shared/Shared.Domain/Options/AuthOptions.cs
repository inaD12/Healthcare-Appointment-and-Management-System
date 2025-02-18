using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Options;

public class AuthOptions
{
	[Required]
	public string SecretKey { get; set; }
	[Required]
	public string Audience { get; set; }
	[Required]
	public string Issuer { get; set; }
	[Required]
	public int SecondsValid { get; set; }
}
