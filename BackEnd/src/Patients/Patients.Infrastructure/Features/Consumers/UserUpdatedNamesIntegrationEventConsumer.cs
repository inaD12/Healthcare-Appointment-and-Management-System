using MassTransit;
using Patients.Application.Features.Patients.Abstractions;
using Serilog;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class UserUpdatedNamesIntegrationEventConsumer(
    IPatientCommandRepository repository,
    IUnitOfWork unitOfWork)
    : IConsumer<UserUpdatedNamesIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserUpdatedNamesIntegrationEvent> context)
    {
        var msg = context.Message;
        
        var patient = await repository.GetByIdAsync(msg.UserId, context.CancellationToken);
        if (patient is null)
        {
            Log.Error($"Name update failed for user {msg.UserId} in UserUpdatedNamesIntegrationEventConsumer.");
            return;
        }
        
        patient.ChangeNames(msg.FirstName, msg.LastName);
        await unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
