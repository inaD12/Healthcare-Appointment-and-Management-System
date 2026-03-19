using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class WorkDayScheduleChangedDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<WorkDayScheduleChangedDomainEvent>
{
    public async Task Consume(ConsumeContext<WorkDayScheduleChangedDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
