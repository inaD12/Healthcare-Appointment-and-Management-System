using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Enums;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentCanceledIntegrationEventConsumer(
    IAppointmentCommandRepository appointmentQueryRepository) 
    : IConsumer<AppointmentCanceledIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentCanceledIntegrationEvent> context)
    {
        await appointmentQueryRepository.UpdateAsync(context.Message.AppointmentId, a =>
        {
            a.Status = AppointmentStatus.Cancelled;
        }, context.CancellationToken);
    }
}
