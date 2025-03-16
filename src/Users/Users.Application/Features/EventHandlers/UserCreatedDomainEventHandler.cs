using MediatR;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Clock;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Domain.Entities;
using Users.Domain.Events;
using Users.Domain.Infrastructure.Abstractions.Repositories;

namespace Users.Application.Features.EventHandlers;

public sealed class UserCreatedDomainEventHandler: INotificationHandler<UserCreatedDomainEvent>
{
	private readonly IEventBus _eventBus;
	private readonly IHAMSMapper _hamsMapper;
	private readonly IEmailVerificationLinkFactory _emailVerificationLinkFactory;
	private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
	private readonly IDateTimeProvider _dateTimeProvider;
	public UserCreatedDomainEventHandler(IEventBus eventBus, IHAMSMapper hamsMapper, IEmailVerificationLinkFactory emailVerificationLinkFactory, IEmailVerificationTokenRepository emailVerificationTokenRepository, IDateTimeProvider dateTimeProvider)
	{
		_eventBus = eventBus;
		_hamsMapper = hamsMapper;
		_emailVerificationLinkFactory = emailVerificationLinkFactory;
		_emailVerificationTokenRepository = emailVerificationTokenRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		var msg = notification;

		var userCreatedEvent = _hamsMapper.Map<UserCreatedIntegrationEvent>(msg);
		await _eventBus.PublishAsync(userCreatedEvent, cancellationToken);

		var emailVerificationToken = EmailVerificationToken.Create(
			msg.Id,
			_dateTimeProvider.UtcNow);

		await _emailVerificationTokenRepository.AddAsync(emailVerificationToken);

		string verificationLink = _emailVerificationLinkFactory.Create(emailVerificationToken);

		var userConfirmEmailEvent = new EmailConfirmationRequestedIntegrationEvent(verificationLink, msg.Email);
		await _eventBus.PublishAsync(userConfirmEmailEvent, cancellationToken);
	}
}
