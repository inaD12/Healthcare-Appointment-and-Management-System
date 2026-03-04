using Doctors.Application.Features.Doctors.Commands.UpdateDoctorNames;
using MassTransit;
using MediatR;
using Serilog;
using Shared.Application.IntegrationEvents;

namespace Doctors.Application.Features.Doctors.Consumers;

public sealed class UserUpdatedNamesIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<UserUpdatedNamesIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserUpdatedNamesIntegrationEvent> context)
    {
        var msg = context.Message;
        
        var command = new UpdateDoctorNamesCommand(msg.UserId, msg.FirstName, msg.LastName);
        var res = await sender.Send(command);
        if (res.IsFailure)
        {
            Log.Error($"Name update failed for user {msg.UserId} in UserUpdatedNamesIntegrationEventConsumer.");
        }
    }
}
