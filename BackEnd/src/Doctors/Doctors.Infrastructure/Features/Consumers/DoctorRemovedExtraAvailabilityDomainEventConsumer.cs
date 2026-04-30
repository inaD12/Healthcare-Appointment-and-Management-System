using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class DoctorRemovedExtraAvailabilityDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<DoctorRemovedExtraAvailabilityDomainEvent>
{
    public async Task Consume(ConsumeContext<DoctorRemovedExtraAvailabilityDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
