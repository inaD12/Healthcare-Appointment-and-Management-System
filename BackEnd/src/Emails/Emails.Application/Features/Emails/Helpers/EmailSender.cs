using Emails.Application.Features.Emails.Abstractions;
using FluentEmail.Core;

namespace Emails.Application.Features.Emails.Helpers;

internal class EmailSender : IEmailSender
{
	private readonly IFluentEmail _fluentEmail;

	public EmailSender(IFluentEmail fluentEmail)
	{
		_fluentEmail = fluentEmail;
	}

	public async Task SendEmailAsync(string email, string subject, string body, bool isHtml = false)
	{
		await _fluentEmail
			   .To(email)
			   .Subject(subject)
			   .Body(body, isHtml)
			   .SendAsync();
	}
}
