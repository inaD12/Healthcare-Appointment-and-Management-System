using Appointments.Domain.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddExtraAvailability;

public sealed class AddExtraAvailabilityCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddExtraAvailabilityCommand>
{
    public async Task<Result> Handle(AddExtraAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorUserId, cancellationToken);

        if (schedule is null)
        {
            schedule = new Domain.Entities.DoctorSchedule(request.DoctorUserId);
            schedule.AddExtraAvailability(request.Start, request.End, request.Reason);

            await scheduleRepository.AddAsync(schedule, cancellationToken);
        }
        else
        {
            schedule.AddExtraAvailability(request.Start, request.End, request.Reason);
            scheduleRepository.Update(schedule);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}