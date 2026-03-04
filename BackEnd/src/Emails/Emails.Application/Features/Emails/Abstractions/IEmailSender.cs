namespace Emails.Application.Features.Emails.Abstractions;

public interface IEmailSender
{
	Task SendEmailAsync(string email, string subject, string body, bool isHtml = false);
}