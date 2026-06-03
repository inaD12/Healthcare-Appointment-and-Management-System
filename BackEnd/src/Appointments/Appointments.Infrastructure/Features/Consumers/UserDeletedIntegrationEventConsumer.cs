using Appointments.Application.Features.DoctorSchedule.Abstractions;
using MassTransit;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Enums;

namespace Appointments.Infrastructure.Features.Consumers;

public sealed class UserDeletedIntegrationEventConsumer(
    IDoctorScheduleRepository  scheduleRepository,
    IUnitOfWork unitOfWork) 
    : IConsumer<UserDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserDeletedIntegrationEvent> context)
    {
        var msg = context.Message;
        if(!msg.Roles.Contains(Roles.Doctor))
            return;

        var schedule = await scheduleRepository.GetByIdAsync(msg.Id);
        if(schedule == null)
            return;
        
        scheduleRepository.Delete(schedule);
        await unitOfWork.SaveChangesAsync();
    }
}
