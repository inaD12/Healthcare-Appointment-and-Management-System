using Appointments.Domain.Events;
using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class AppointmentRescheduledDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<AppointmentRescheduledDomainEvent>
{
    public async Task Consume(ConsumeContext<AppointmentRescheduledDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
