using MassTransit;
using Patients.Application.Features.Patients.Abstractions;
using Patients.Domain.Entities;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentCreatedIntegrationEventConsumer(
    IAppointmentCommandRepository appointmentQueryRepository) 
    : IConsumer<AppointmentCreatedIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentCreatedIntegrationEvent> context)
    {
        var projection = new AppointmentProjection
        {
            Id = context.Message.AppointmentId,
            DoctorId = context.Message.DoctorId,
            PatientId = context.Message.PatientId,
            Start = context.Message.Start.AddHours(-3),
            End = context.Message.End.AddHours(-3)
        };

        await appointmentQueryRepository.UpsertAsync(projection, context.CancellationToken);
    }
}
