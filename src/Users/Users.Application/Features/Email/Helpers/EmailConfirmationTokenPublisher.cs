using Shared.Domain.Events;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Email.Models;
using Users.Application.Features.Managers.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Features.Email.Helpers;

internal class EmailConfirmationTokenPublisher : IEmailConfirmationTokenPublisher
{
	private readonly IFactoryManager _factoryManager;
	private readonly IRepositoryManager _repositoryManager;
	private readonly IEventBus _eventBus;
	public EmailConfirmationTokenPublisher(IFactoryManager factoryManager, IRepositoryManager repositoryManager, IEventBus eventBus)
	{
		_factoryManager = factoryManager;
		_repositoryManager = repositoryManager;
		_eventBus = eventBus;
	}

	public async Task PublishEmailConfirmationTokenAsync(PublishEmailConfirmationTokenModel model)
	{
		DateTime utcNow = DateTime.UtcNow;

		EmailVerificationToken emailVerificationToken = _factoryManager.EmailTokenFactory.CreateToken(
			model.UserId,
			utcNow);

		await _repositoryManager.EmailVerificationToken.AddAsync(emailVerificationToken);

		string verificationLink = _factoryManager.EmailLinkFactory.Create(emailVerificationToken);

		var userConfirmEmailEvent = new EmailConfirmationRequestedEvent(model.Email, verificationLink);

		await _eventBus.PublishAsync(userConfirmEmailEvent);
	}
}
