using Appointments.Application.Features.Mappers;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;

namespace Appointments.Application.Features.Consumers;

public sealed class UserCreatedIntegrationConsumer : IConsumer<UserCreatedIntegrationEvent>
{
	private readonly IUserDataRepository _userDataRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UserCreatedIntegrationConsumer(IUnitOfWork unitOfWork, IUserDataRepository userDataRepository)
	{
		_unitOfWork = unitOfWork;
		_userDataRepository = userDataRepository;
	}

	public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
	{
		var userData = context.Message.ToUserData();

		await _userDataRepository.AddAsync(userData);

		await _unitOfWork.SaveChangesAsync();
	}
}
