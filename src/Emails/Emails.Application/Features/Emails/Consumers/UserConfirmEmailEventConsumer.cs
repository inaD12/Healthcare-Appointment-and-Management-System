using Emails.Application.Features.Emails.Abstractions;
using Emails.Application.Features.Emails.Utilities;
using MassTransit;
using Shared.Domain.Events;

namespace Emails.Application.Features.Emails.Consumers
{
	public sealed class UserConfirmEmailEventConsumer: IConsumer<EmailConfirmationRequestedEvent>
	{
		private readonly IEmailSender _emailSender;

		public UserConfirmEmailEventConsumer(IEmailSender emailSender)
		{
			_emailSender = emailSender;
		}

		public async Task Consume(ConsumeContext<EmailConfirmationRequestedEvent> context)
		{
			var message = context.Message;

			string body = $"To verify your email <a href='{message.link}'>click here</a>";

			await _emailSender.SendEmailAsync(message.email, Constants.EmailConfirmationTitle, body, true);
		}
	}
}
