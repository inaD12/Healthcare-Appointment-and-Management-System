using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Domain.Events;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Application.Features.Appointments.Consumers;

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
