using Appointments.Domain.Events;
using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class AppointmentCreatedDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<AppointmentCreatedDomainEvent>
{
    public async Task Consume(ConsumeContext<AppointmentCreatedDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
