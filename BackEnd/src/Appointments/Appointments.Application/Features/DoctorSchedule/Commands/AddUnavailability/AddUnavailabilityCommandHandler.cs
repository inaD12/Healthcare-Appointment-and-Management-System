using Appointments.Application.Features.DoctorSchedule.Abstractions;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddUnavailability;

public sealed class AddDoctorUnavailabilityCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddUnavailabilityCommand>
{
    public async Task<Result> Handle(AddUnavailabilityCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorUserId, cancellationToken);

        if (schedule is null)
        {
            schedule = new Domain.Entities.DoctorSchedule(request.DoctorUserId);
            schedule.AddUnavailability(request.Start, request.End, request.Reason);

            await scheduleRepository.AddAsync(schedule, cancellationToken);
        }
        else
        {
            schedule.AddUnavailability(request.Start, request.End, request.Reason);
            scheduleRepository.Update(schedule);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}