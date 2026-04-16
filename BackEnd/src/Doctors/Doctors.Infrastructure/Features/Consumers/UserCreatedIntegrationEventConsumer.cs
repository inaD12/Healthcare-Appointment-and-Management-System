using Doctors.Application.Features.Doctors.Commands.CreateDoctor;
using MassTransit;
using MediatR;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Enums;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class UserCreatedIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<UserCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        var msg = context.Message;
        if(!msg.Roles.Contains(Roles.Doctor))
            return;
        
        var command = new CreateDoctorCommand(msg.Id, msg.FirstName, msg.LastName, null, null);
        await sender.Send(command);
    }
}
