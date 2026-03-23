using MassTransit;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.ReadModels;
using Shared.Application.IntegrationEvents;

namespace Patients.Infrastructure.Features.Consumers;

public sealed class AppointmentCreatedIntegrationEventConsumer(
    IAppointmentReadRepository appointmentReadRepository) 
    : IConsumer<AppointmentCreatedIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<AppointmentCreatedIntegrationEvent> context)
    {
        var projection = new AppointmentProjection
        {
            Id = context.Message.AppointmentId,
            DoctorId = context.Message.DoctorId,
            PatientId = context.Message.PatientId,
            Start = context.Message.Start,
            End = context.Message.End
        };

        await appointmentReadRepository.UpsertAsync(projection, context.CancellationToken);
    }
}
