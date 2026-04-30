using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class WorkDayScheduleAddedIntegrationEventConsumer(
    ISender sender)
    : IConsumer<WorkDayScheduleAddedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<WorkDayScheduleAddedIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
