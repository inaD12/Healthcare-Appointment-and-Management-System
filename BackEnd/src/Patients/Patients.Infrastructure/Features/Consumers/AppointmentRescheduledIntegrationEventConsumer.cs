using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Enums;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentRescheduledIntegrationEventConsumer(
    IAppointmentReadRepository appointmentReadRepository) 
    : IConsumer<AppointmentRescheduledIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentRescheduledIntegrationEvent> context)
    {
        await appointmentReadRepository.UpdateAsync(context.Message.AppointmentId, a =>
        {
            a.Status = AppointmentStatus.Rescheduled;
        }, context.CancellationToken);
    }
}
