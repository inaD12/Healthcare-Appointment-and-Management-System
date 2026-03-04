using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveExtraAvailability;

public sealed class RemoveExtraAvailabilityCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveExtraAvailabilityCommand>
{
    public async Task<Result> Handle(RemoveExtraAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorId, cancellationToken);
        if (schedule == null)
            return Result.Failure(ResponseList.ScheduleNotFound);
        
        schedule.RemoveExtraAvailability(request.Start, request.End);
        await scheduleRepository.AddAsync(schedule, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}