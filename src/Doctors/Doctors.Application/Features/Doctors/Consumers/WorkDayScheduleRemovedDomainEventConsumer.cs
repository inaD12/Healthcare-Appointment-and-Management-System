using Doctors.Application.Features.Doctors.Mappers;
using Doctors.Domain.Events;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Application.Features.Doctors.Consumers;

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
