using MassTransit;
using MediatR;
using Ratings.Application.Features.RateableAppointments.Commands.AddAppointment;
using Serilog;
using Shared.Application.IntegrationEvents;

namespace Ratings.Application.Features.RateableAppointments.Consumers;

public sealed class AppointmentCompletedIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<AppointmentCompletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<AppointmentCompletedIntegrationEvent> context)
    {
        var msg = context.Message;
        
        var command = new AddAppointmentCommand(msg.AppointmentId, msg.DoctorId, msg.PatientId);
        var res = await sender.Send(command);
        if (res.IsFailure)
        {
            Log.Error($"Adding rateable appointment failed for {msg.AppointmentId} in AppointmentCompletedIntegrationEventConsumer.");
        }
    }
}
