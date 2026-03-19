using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class DoctorAddedExtraAvailabilityIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<DoctorAddedExtraAvailabilityIntegrationEvent>
{
    public async Task Consume(ConsumeContext<DoctorAddedExtraAvailabilityIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
