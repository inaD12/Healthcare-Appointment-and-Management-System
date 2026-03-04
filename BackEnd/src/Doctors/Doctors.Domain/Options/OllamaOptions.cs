using System.ComponentModel.DataAnnotations;

namespace Doctors.Domain.Options;

public sealed class OllamaOptions
{
	[Required]
	public string Url { get; set; } = string.Empty;
	[Required]
	public string Model { get; set; } = string.Empty;
}
