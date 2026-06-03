using MassTransit;
using Shared.Application.Abstractions;
using Users.Domain.Events;
using Users.Infrastructure.Features.Mappers;

namespace Users.Infrastructure.Features.Consumers;

public sealed class UserDeletedDomainEventConsumer(
	IEventBus eventBus)
	: IConsumer<UserDeletedDomainEvent>
{
	public async Task Consume(ConsumeContext<UserDeletedDomainEvent> context)
	{
		var msg = context.Message;

		var userDeletedEvent = msg.ToIntEvent();

		await eventBus.PublishAsync(userDeletedEvent, context.CancellationToken);
	}
}
