using Doctors.Application.Features.Doctors.Mappers;
using Doctors.Domain.Events;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Application.Features.Doctors.Consumers;

public sealed class DoctorRemovedUnavailabilityDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<DoctorRemovedUnavailabilityDomainEvent>
{
    public async Task Consume(ConsumeContext<DoctorRemovedUnavailabilityDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
