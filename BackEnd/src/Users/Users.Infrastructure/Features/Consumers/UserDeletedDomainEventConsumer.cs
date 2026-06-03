using MassTransit;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;
using Users.Domain.Events;

namespace Users.Infrastructure.Features.Consumers;

public sealed class UserDeletedDomainEventConsumer(
	IEventBus eventBus)
	: IConsumer<UserDeletedDomainEvent>
{
	public async Task Consume(ConsumeContext<UserDeletedDomainEvent> context)
	{
		var msg = context.Message;

		var userDeletedEvent = new UserDeletedIntegrationEvent(msg.Id);

		await eventBus.PublishAsync(userDeletedEvent, context.CancellationToken);
	}
}
