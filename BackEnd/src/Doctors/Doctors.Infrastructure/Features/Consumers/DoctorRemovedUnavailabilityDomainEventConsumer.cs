using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

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
