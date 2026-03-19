using Appointments.Domain.Events;
using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class AppointmentCanceledDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<AppointmentCanceledDomainEvent>
{
    public async Task Consume(ConsumeContext<AppointmentCanceledDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
