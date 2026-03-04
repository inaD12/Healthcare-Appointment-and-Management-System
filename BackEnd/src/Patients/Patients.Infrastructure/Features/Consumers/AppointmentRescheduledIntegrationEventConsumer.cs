using MassTransit;
using Patients.Infrastructure.Features.ReadModels;
using Patients.Infrastructure.Features.ReadModels.Abstractions;
using Patients.Infrastructure.Features.ReadModels.Enums;
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
