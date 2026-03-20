using MassTransit;
using Shared.Domain.Abstractions;
using Users.Domain.Events;
using Users.Infrastructure.Features.Mappers;

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
