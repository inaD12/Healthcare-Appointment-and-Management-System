using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Domain.Events;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Appointments.Application.Features.Appointments.Consumers;

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
