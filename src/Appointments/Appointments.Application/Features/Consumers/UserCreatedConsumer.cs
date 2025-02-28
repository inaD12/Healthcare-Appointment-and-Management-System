using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Entities;
using MassTransit;
using Shared.Application.Abstractions;
using Shared.Domain.Events;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UserCreatedConsumer(IRepositoryManager repositoryManager, IHAMSMapper mapper, IUnitOfWork unitOfWork)
	{
		_repositoryManager = repositoryManager;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task Consume(ConsumeContext<UserCreatedEvent> context)
	{
		var userData = _mapper.Map<UserData>(context.Message);

		await _repositoryManager.UserData.AddAsync(userData);

		await _unitOfWork.SaveChangesAsync();
	}
}
