using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Entities;
using MassTransit;
using Shared.Application.Abstractions;
using Shared.Domain.Events;

namespace Appointments.Application.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IHAMSMapper _mapper;

	public UserCreatedConsumer(IRepositoryManager repositoryManager, IHAMSMapper mapper)
	{
		_repositoryManager = repositoryManager;
		_mapper = mapper;
	}

	public async Task Consume(ConsumeContext<UserCreatedEvent> context)
	{
		var userData = _mapper.Map<UserData>(context.Message);

		await _repositoryManager.UserData.AddAsync(userData);
	}
}
