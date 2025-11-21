using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Clock;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Users.Mappings;
using Users.Domain.Entities;
using Users.Domain.Events;
using Users.Domain.Infrastructure.Abstractions.Repositories;

namespace Users.Application.Features.Users.Consumers;

public sealed class UserCreatedDomainEventConsumer : IConsumer<UserCreatedDomainEvent>
{
	private readonly IEventBus _eventBus;
	private readonly IEmailVerificationLinkFactory _emailVerificationLinkFactory;
	private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public UserCreatedDomainEventConsumer(
		IEventBus eventBus,
		IEmailVerificationLinkFactory emailVerificationLinkFactory,
		IEmailVerificationTokenRepository emailVerificationTokenRepository,
		IDateTimeProvider dateTimeProvider)
	{
		_eventBus = eventBus;
		_emailVerificationLinkFactory = emailVerificationLinkFactory;
		_emailVerificationTokenRepository = emailVerificationTokenRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task Consume(ConsumeContext<UserCreatedDomainEvent> context)
	{
		var msg = context.Message;

		var userCreatedEvent = msg.ToIntEvent();
		var userCreatedEventTask = _eventBus.PublishAsync(userCreatedEvent, context.CancellationToken);

		var emailVerificationToken = EmailVerificationToken.Create(msg.Id, _dateTimeProvider.UtcNow);
		await _emailVerificationTokenRepository.AddAsync(emailVerificationToken);

		string verificationLink = _emailVerificationLinkFactory.Create(emailVerificationToken);
		var userConfirmEmailEvent = new EmailConfirmationRequestedIntegrationEvent(verificationLink, msg.Email);

		var emailEventTask = _eventBus.PublishAsync(userConfirmEmailEvent, context.CancellationToken);

		await Task.WhenAll(userCreatedEventTask, emailEventTask);
	}

}
