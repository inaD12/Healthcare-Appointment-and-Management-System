using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.Entities;
using MassTransit;
using Shared.Application.Abstractions;
using Shared.Domain.Events;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
	private readonly IUserDataRepository _userDataRepository;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UserCreatedConsumer(IHAMSMapper mapper, IUnitOfWork unitOfWork, IUserDataRepository userDataRepository)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userDataRepository = userDataRepository;
	}

	public async Task Consume(ConsumeContext<UserCreatedEvent> context)
	{
		var userData = _mapper.Map<UserData>(context.Message);

		await _userDataRepository.AddAsync(userData);

		await _unitOfWork.SaveChangesAsync();
	}
}
