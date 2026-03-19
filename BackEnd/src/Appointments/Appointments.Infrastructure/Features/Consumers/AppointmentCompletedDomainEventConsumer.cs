using Appointments.Domain.Events;
using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class AppointmentCompletedDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<AppointmentCompletedDomainEvent>
{
    public async Task Consume(ConsumeContext<AppointmentCompletedDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
