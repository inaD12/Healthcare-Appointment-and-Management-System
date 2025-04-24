using System.ComponentModel.DataAnnotations;

namespace Shared.API.Options;

public class CorsOptions
{
	[Required]
	public string AllowedOrigins { get; set; } = string.Empty;
}
