using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Options;

public sealed class DatabaseOptions
{
	[Required]
	public string ConnectionString { get; set; } = string.Empty;
}
