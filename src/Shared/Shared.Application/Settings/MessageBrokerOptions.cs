using System.ComponentModel.DataAnnotations;

namespace Shared.Application.Settings;

public sealed class MessageBrokerOptions
{
	[Required]
	public string Host { get; set; } = string.Empty;
	[Required]
	public string Username { get; set; } = string.Empty;
	[Required]
	public string Password { get; set; } = string.Empty;
}
