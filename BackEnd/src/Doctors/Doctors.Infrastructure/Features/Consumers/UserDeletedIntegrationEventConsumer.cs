using Doctors.Application.Features.Doctors.Abstractions;
using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Enums;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class UserDeletedIntegrationEventConsumer(
    IDoctorRepository  doctorRepository) 
    : IConsumer<UserDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserDeletedIntegrationEvent> context)
    {
        var msg = context.Message;
        if(!msg.Roles.Contains(Roles.Doctor))
            return;

        var doctor = await doctorRepository.GetByUserIdAsync(msg.Id);
        if(doctor == null)
            return;
        
        doctorRepository.Delete(doctor);
    }
}
