using MassTransit;
using Shared.Application.Abstractions;
using Users.Domain.Events;
using Users.Infrastructure.Features.Mappers;

namespace Users.Infrastructure.Features.Consumers;

public sealed class UserUpdatedNamesDomainEventConsumer(IEventBus eventBus) : IConsumer<UserUpdatedNamesDomainEvent>
{
	public async Task Consume(ConsumeContext<UserUpdatedNamesDomainEvent> context)
	{
		var msg = context.Message;
		var userUpdatedNamesEvent = msg.ToIntegrationEvent();
		await eventBus.PublishAsync(userUpdatedNamesEvent, context.CancellationToken);
	}
}
