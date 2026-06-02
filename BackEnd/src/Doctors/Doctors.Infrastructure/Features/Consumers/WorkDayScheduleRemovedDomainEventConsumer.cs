using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Application.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class WorkDayScheduleRemovedDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<WorkDayScheduleRemovedDomainEvent>
{
    public async Task Consume(ConsumeContext<WorkDayScheduleRemovedDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
