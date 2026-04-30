using Appointments.Domain.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.ChangeWorkDaySchedule;

public sealed class ChangeWorkDayScheduleCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeWorkDayScheduleCommand>
{
    public async Task<Result> Handle(ChangeWorkDayScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorUserId, cancellationToken);

        if (schedule is null)
        {
            schedule = new Domain.Entities.DoctorSchedule(request.DoctorUserId);
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