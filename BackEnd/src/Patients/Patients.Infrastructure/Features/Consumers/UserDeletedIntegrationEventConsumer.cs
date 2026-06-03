using MassTransit;
using MediatR;
using Patients.Application.Features.Patients.Commands.DeletePatient;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Enums;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class UserDeletedIntegrationEventConsumer(
    ISender sender) 
    : IConsumer<UserDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserDeletedIntegrationEvent> context)
    {
        var msg = context.Message;
        if(!msg.Roles.Contains(Roles.Patient))
            return;

        var command = new DeletePatientCommand(msg.Id);
        await sender.Send(command);
    }
}
