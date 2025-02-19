using System.ComponentModel.DataAnnotations;

namespace Emails.Application.Features.Emails.Options;

public class EmailOptions
{
	[Required]
	public string SenderEmail { get; set; } = string.Empty;
	[Required]
	public string Sender {  get; set; } = string.Empty;
	[Required]
	public string Host {  get; set; } = string.Empty;
	[Required]
	public int Port { get; set; }
}
