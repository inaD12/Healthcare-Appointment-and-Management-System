using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Domain.Events;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Application.Features.Appointments.Consumers;

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
