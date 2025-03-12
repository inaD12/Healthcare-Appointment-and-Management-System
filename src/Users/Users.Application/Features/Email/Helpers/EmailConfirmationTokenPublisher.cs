using Shared.Domain.Events;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Email.Models;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Entities;

namespace Users.Application.Features.Email.Helpers;

internal class EmailConfirmationTokenPublisher : IEmailConfirmationTokenPublisher
{
	private readonly IEmailVerificationTokenFactory _emailVerificationTokenFactory;
	private readonly IEmailVerificationLinkFactory _emailVerificationLinkFactory;
	private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
	private readonly IEventBus _eventBus;
	public EmailConfirmationTokenPublisher(IEventBus eventBus, IEmailVerificationTokenFactory emailVerificationTokenFactory, IEmailVerificationLinkFactory emailVerificationLinkFactory, IEmailVerificationTokenRepository emailVerificationTokenRepository)
	{
		_eventBus = eventBus;
		_emailVerificationTokenFactory = emailVerificationTokenFactory;
		_emailVerificationLinkFactory = emailVerificationLinkFactory;
		_emailVerificationTokenRepository = emailVerificationTokenRepository;
	}

	public async Task PublishEmailConfirmationTokenAsync(PublishEmailConfirmationTokenModel model)
	{
		DateTime utcNow = DateTime.UtcNow;

		EmailVerificationToken emailVerificationToken = _emailVerificationTokenFactory.CreateToken(
			model.UserId,
			utcNow);

		await _emailVerificationTokenRepository.AddAsync(emailVerificationToken);

		string verificationLink = _emailVerificationLinkFactory.Create(emailVerificationToken);

		var userConfirmEmailEvent = new EmailConfirmationRequestedEvent(model.Email, verificationLink);

		await _eventBus.PublishAsync(userConfirmEmailEvent);
	}
}
