using MassTransit;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Clock;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Domain.Entities;
using Users.Domain.Events;
using Users.Domain.Infrastructure.Abstractions.Repositories;

namespace Users.Application.Features.Users.Consumers;

public sealed class UserCreatedDomainEventConsumer : IConsumer<UserCreatedDomainEvent>
{
	private readonly IEventBus _eventBus;
	private readonly IHAMSMapper _hamsMapper;
	private readonly IEmailVerificationLinkFactory _emailVerificationLinkFactory;
	private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
	private readonly IDateTimeProvider _dateTimeProvider;

	public UserCreatedDomainEventConsumer(
		IEventBus eventBus,
		IHAMSMapper hamsMapper,
		IEmailVerificationLinkFactory emailVerificationLinkFactory,
		IEmailVerificationTokenRepository emailVerificationTokenRepository,
		IDateTimeProvider dateTimeProvider)
	{
		_eventBus = eventBus;
		_hamsMapper = hamsMapper;
		_emailVerificationLinkFactory = emailVerificationLinkFactory;
		_emailVerificationTokenRepository = emailVerificationTokenRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task Consume(ConsumeContext<UserCreatedDomainEvent> context)
	{
		var msg = context.Message;

		var userCreatedEvent = _hamsMapper.Map<UserCreatedIntegrationEvent>(msg);
		var userCreatedEventTask = _eventBus.PublishAsync(userCreatedEvent, context.CancellationToken);

		var emailVerificationToken = EmailVerificationToken.Create(msg.Id, _dateTimeProvider.UtcNow);
		await _emailVerificationTokenRepository.AddAsync(emailVerificationToken);

		string verificationLink = _emailVerificationLinkFactory.Create(emailVerificationToken);
		var userConfirmEmailEvent = new EmailConfirmationRequestedIntegrationEvent(verificationLink, msg.Email);

		var emailEventTask = _eventBus.PublishAsync(userConfirmEmailEvent, context.CancellationToken);

		await Task.WhenAll(userCreatedEventTask, emailEventTask);
	}

}
