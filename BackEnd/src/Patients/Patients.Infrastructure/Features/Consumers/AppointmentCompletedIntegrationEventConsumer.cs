using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Enums;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentCompletedIntegrationEventConsumer(
    IAppointmentReadRepository appointmentReadRepository) 
    : IConsumer<AppointmentCompletedIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentCompletedIntegrationEvent> context)
    {
        await appointmentReadRepository.UpdateAsync(context.Message.AppointmentId, a =>
        {
            a.Status = AppointmentStatus.Completed;
        }, context.CancellationToken);
    }
}
