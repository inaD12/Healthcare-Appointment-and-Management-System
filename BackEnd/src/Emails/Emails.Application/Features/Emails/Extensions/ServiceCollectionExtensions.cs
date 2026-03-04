using Emails.Application.Features.Emails.Abstractions;
using Emails.Application.Features.Emails.Helpers;
using Emails.Application.Features.Emails.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Emails.Application.Features.Emails.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddOptions<EmailOptions>()
			.BindConfiguration(nameof(EmailOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		var emailOptions = configuration
			.GetSection(nameof(EmailOptions))
			.Get<EmailOptions>()!;

		services
			.AddTransient<IEmailSender, EmailSender>();

		services
			.AddFluentEmail(emailOptions.SenderEmail, emailOptions.Sender)
			.AddSmtpSender(emailOptions.Host, emailOptions.Port);

		return services;
	}
}
