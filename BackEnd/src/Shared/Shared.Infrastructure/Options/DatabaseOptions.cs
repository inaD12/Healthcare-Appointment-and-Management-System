using System.ComponentModel.DataAnnotations;

namespace Shared.Infrastructure.Options;

public sealed class DatabaseOptions
{
	[Required]
	public string ConnectionString { get; set; } = string.Empty;
}
