using Appointments.Domain.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddWorkDaySchedule;

public sealed class AddWorkDayScheduleCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddWorkDayScheduleCommand>
{
    public async Task<Result> Handle(AddWorkDayScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorId, cancellationToken);

        if (schedule is null)
        {
            schedule = new Domain.Entities.DoctorSchedule(request.DoctorId);
            schedule.AddOrUpdateWorkDay(request.DayOfWeek, request.WorkTimes);

            await scheduleRepository.AddAsync(schedule, cancellationToken);
        }
        else
        {
            schedule.AddOrUpdateWorkDay(request.DayOfWeek, request.WorkTimes);
            scheduleRepository.Update(schedule);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}