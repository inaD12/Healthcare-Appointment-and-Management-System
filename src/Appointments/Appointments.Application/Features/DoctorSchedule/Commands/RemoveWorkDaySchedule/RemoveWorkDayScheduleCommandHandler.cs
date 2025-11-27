using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveWorkDaySchedule;

public sealed class RemoveWorkDayScheduleCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveWorkDayScheduleCommand>
{
    public async Task<Result> Handle(RemoveWorkDayScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorId, cancellationToken);
        if (schedule == null)
            return Result.Failure(ResponseList.ScheduleNotFound);
        
        schedule.RemoveWorkDay(request.DayOfWeek);
        await scheduleRepository.AddAsync(schedule, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}