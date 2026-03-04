using Appointments.Application.Features.DoctorSchedule.Mappers;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;

namespace Appointments.Application.Features.DoctorSchedule.Consumers;

public sealed class WorkDayScheduleRemovedIntegrationEventConsumer(
    ISender sender)
    : IConsumer<WorkDayScheduleRemovedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<WorkDayScheduleRemovedIntegrationEvent> context)
    {
        var command = context.Message.ToCommand();
        
        await sender.Send(command, context.CancellationToken);
    }
}
