using Appointments.Application.Features.DoctorSchedule.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Application.Features.DoctorSchedule.Consumers;

public sealed class DoctorAddedUnavailabilityIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<DoctorAddedUnavailabilityIntegrationEvent>
{
    public async Task Consume(ConsumeContext<DoctorAddedUnavailabilityIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
