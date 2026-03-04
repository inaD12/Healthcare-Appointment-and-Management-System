using MassTransit;
using Shared.Domain.Abstractions;
using Users.Application.Features.Users.Mappers;
using Users.Domain.Events;

namespace Users.Application.Features.Users.Consumers;

public sealed class UserUpdatedNamesDomainEventConsumer(IEventBus eventBus) : IConsumer<UserUpdatedNamesDomainEvent>
{
	public async Task Consume(ConsumeContext<UserUpdatedNamesDomainEvent> context)
	{
		var msg = context.Message;
		var userUpdatedNamesEvent = msg.ToIntegrationEvent();
		await eventBus.PublishAsync(userUpdatedNamesEvent, context.CancellationToken);
	}
}
