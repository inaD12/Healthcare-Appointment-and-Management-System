using Doctors.Domain.Events;
using Doctors.Infrastructure.Features.Mappers;
using MassTransit;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class DoctorAddedExtraAvailabilityDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<DoctorAddedExtraAvailabilityDomainEvent>
{
    public async Task Consume(ConsumeContext<DoctorAddedExtraAvailabilityDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
