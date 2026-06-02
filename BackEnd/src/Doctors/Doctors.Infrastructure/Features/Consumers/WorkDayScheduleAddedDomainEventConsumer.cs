using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Application.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class WorkDayScheduleAddedDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<WorkDayScheduleAddedDomainEvent>
{
    public async Task Consume(ConsumeContext<WorkDayScheduleAddedDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
