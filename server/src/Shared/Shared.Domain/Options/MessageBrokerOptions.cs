using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Options;

public sealed class MessageBrokerOptions
{
	[Required]
	public string Host { get; set; } = string.Empty;
	[Required]
	public string Username { get; set; } = string.Empty;
	[Required]
	public string Password { get; set; } = string.Empty;
}
