using MassTransit;
using Ratings.Application.Features.Ratings.Mappers;
using Ratings.Domain.Events;
using Shared.Domain.Abstractions;

namespace Ratings.Infrastructure.Features.Consumers;

public sealed class DoctorAverageRatingUpdatedDomainEventConsumer(
    IEventBus eventBus) 
    : IConsumer<DoctorAverageRatingUpdatedDomainEvent>
{
    public async Task Consume(ConsumeContext<DoctorAverageRatingUpdatedDomainEvent> context)
    {
        var integrationEvent = context.Message.ToIntegrationEvent();

        await eventBus.PublishAsync(integrationEvent, context.CancellationToken);
    }
}
