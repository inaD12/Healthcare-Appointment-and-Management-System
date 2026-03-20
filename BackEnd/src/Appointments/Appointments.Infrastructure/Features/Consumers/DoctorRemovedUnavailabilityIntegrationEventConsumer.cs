using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class DoctorRemovedUnavailabilityIntegrationEventConsumer(
    ISender sender)
    : IConsumer<DoctorRemovedUnavailabilityIntegrationEvent>
{
    public async Task Consume(ConsumeContext<DoctorRemovedUnavailabilityIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
