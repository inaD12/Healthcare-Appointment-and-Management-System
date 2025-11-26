using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveUnavailability;

public sealed class RemoveDoctorUnavailabilityCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveUnavailabilityCommand>
{
    public async Task<Result> Handle(RemoveUnavailabilityCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorId, cancellationToken);
        if (schedule == null)
            return Result.Failure(ResponseList.ScheduleNotFound);
        
        schedule.RemoveUnavailability(request.Start, request.End);
        await scheduleRepository.AddAsync(schedule, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}