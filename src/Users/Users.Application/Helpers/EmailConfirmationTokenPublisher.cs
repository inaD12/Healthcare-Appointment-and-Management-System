using Shared.Infrastructure.MessageBroker;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Helpers;

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

	public async Task PublishEmailConfirmationTokenAsync(string email, string userId)
	{
		DateTime utcNow = DateTime.UtcNow;

		EmailVerificationToken emailVerificationToken = _factoryManager.EmailTokenFactory.CreateToken(
			Guid.NewGuid().ToString(),
			userId,
			utcNow,
			utcNow.AddDays(1));

		await _repositoryManager.EmailVerificationToken.AddAsync(emailVerificationToken);

		string verificationLink = _factoryManager.EmailLinkFactory.Create(emailVerificationToken);

		var userConfirmEmailEvent = _factoryManager.UserConfirmEmailEventFactory.CreateUserConfirmEmailEvent(email, verificationLink);

		await _eventBus.PublishAsync(userConfirmEmailEvent);
	}
}
