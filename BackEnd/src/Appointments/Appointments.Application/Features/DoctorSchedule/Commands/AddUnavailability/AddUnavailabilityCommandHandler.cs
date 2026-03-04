using Appointments.Domain.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddUnavailability;

public sealed class AddDoctorUnavailabilityCommandHandler(
    IDoctorScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddUnavailabilityCommand>
{
    public async Task<Result> Handle(AddUnavailabilityCommand request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.GetByIdAsync(request.DoctorId, cancellationToken);

        if (schedule is null)
        {
            schedule = new Domain.Entities.DoctorSchedule(request.DoctorId);
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