using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Enums;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentCanceledIntegrationEventConsumer(
    IAppointmentReadRepository appointmentReadRepository) 
    : IConsumer<AppointmentCanceledIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentCanceledIntegrationEvent> context)
    {
        await appointmentReadRepository.UpdateAsync(context.Message.AppointmentId, a =>
        {
            a.Status = AppointmentStatus.Cancelled;
        }, context.CancellationToken);
    }
}
