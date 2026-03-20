using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class DoctorAddedUnavailabilityDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<DoctorAddedUnavailabilityDomainEvent>
{
    public async Task Consume(ConsumeContext<DoctorAddedUnavailabilityDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
