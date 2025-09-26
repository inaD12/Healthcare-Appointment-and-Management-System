using Appointments.Domain.Entities;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using MassTransit;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;

namespace Appointments.Application.Features.Consumers;

public sealed class UserCreatedIntegrationConsumer : IConsumer<UserCreatedIntegrationEvent>
{
	private readonly IUserDataRepository _userDataRepository;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UserCreatedIntegrationConsumer(IHAMSMapper mapper, IUnitOfWork unitOfWork, IUserDataRepository userDataRepository)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userDataRepository = userDataRepository;
	}

	public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
	{
		var userData = _mapper.Map<UserData>(context.Message);

		await _userDataRepository.AddAsync(userData);

		await _unitOfWork.SaveChangesAsync();
	}
}
