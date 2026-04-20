using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Enums;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentRescheduledIntegrationEventConsumer(
    IAppointmentCommandRepository appointmentQueryRepository) 
    : IConsumer<AppointmentRescheduledIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentRescheduledIntegrationEvent> context)
    {
        await appointmentQueryRepository.UpdateAsync(context.Message.AppointmentId, a =>
        {
            a.Status = AppointmentStatus.Rescheduled;
        }, context.CancellationToken);
    }
}
