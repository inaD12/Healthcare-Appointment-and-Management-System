using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class DoctorRemovedExtraAvailabilityIntegrationEventConsumer(
    ISender sender)
    : IConsumer<DoctorRemovedExtraAvailabilityIntegrationEvent>
{
    public async Task Consume(ConsumeContext<DoctorRemovedExtraAvailabilityIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
