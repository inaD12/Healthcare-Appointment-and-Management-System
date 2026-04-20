using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Enums;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentCompletedIntegrationEventConsumer(
    IAppointmentCommandRepository appointmentQueryRepository) 
    : IConsumer<AppointmentCompletedIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentCompletedIntegrationEvent> context)
    {
        await appointmentQueryRepository.UpdateAsync(context.Message.AppointmentId, a =>
        {
            a.Status = AppointmentStatus.Completed;
        }, context.CancellationToken);
    }
}
