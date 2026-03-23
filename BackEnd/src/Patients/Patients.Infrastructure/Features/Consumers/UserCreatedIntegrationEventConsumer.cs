using MassTransit;
using MediatR;
using Patients.Application.Features.Patients.Commands.RegisterPatient;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class UserCreatedIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<UserCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        var msg = context.Message;
        
        var command = new RegisterPatientCommand(msg.Id, msg.FirstName, msg.LastName, msg.BirthDay);
        await sender.Send(command);
    }
}
