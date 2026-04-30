using Appointments.Infrastructure.Features.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class WorkDayScheduleChangedIntegrationEventConsumer(
    ISender sender)
    : IConsumer<WorkDayScheduleChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<WorkDayScheduleChangedIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
