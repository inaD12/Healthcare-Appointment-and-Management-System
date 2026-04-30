using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Clock;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Entities;
using Users.Domain.Events;
using Users.Infrastructure.Features.Helpers.Abstractions;
using Users.Infrastructure.Features.Mappers;

namespace Users.Infrastructure.Features.Consumers;

public sealed class UserCreatedDomainEventConsumer(
	IEventBus eventBus,
	IEmailVerificationLinkFactory emailVerificationLinkFactory,
	IEmailVerificationTokenRepository emailVerificationTokenRepository,
	IDateTimeProvider dateTimeProvider)
	: IConsumer<UserCreatedDomainEvent>
{
	public async Task Consume(ConsumeContext<UserCreatedDomainEvent> context)
	{
		var msg = context.Message;

		var userCreatedEvent = msg.ToIntEvent();
		var userCreatedEventTask = eventBus.PublishAsync(userCreatedEvent, context.CancellationToken);

		var emailVerificationToken = EmailVerificationToken.Create(msg.Id, dateTimeProvider.UtcNow);
		await emailVerificationTokenRepository.AddAsync(emailVerificationToken);

		string verificationLink = emailVerificationLinkFactory.Create(emailVerificationToken);
		var userConfirmEmailEvent = new EmailConfirmationRequestedIntegrationEvent(verificationLink, msg.Email);

		var emailEventTask = eventBus.PublishAsync(userConfirmEmailEvent, context.CancellationToken);

		await Task.WhenAll(userCreatedEventTask, emailEventTask);
	}

}
