using Appointments.Application.Managers.Interfaces;
using MassTransit;
using Shared.Domain.Events;

namespace Appointments.Application.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IFactoryManager _factoryManager;

	public UserCreatedConsumer(IRepositoryManager repositoryManager, IFactoryManager factoryManager)
	{
		_repositoryManager = repositoryManager;
		_factoryManager = factoryManager;
	}

	public async Task Consume(ConsumeContext<UserCreatedEvent> context)
	{
		var UserData = _factoryManager.UserData.Create(
			context.Message.UserId,
			context.Message.Email,
			context.Message.Role);

		await _repositoryManager.UserData.AddAsync(UserData);
	}
}
