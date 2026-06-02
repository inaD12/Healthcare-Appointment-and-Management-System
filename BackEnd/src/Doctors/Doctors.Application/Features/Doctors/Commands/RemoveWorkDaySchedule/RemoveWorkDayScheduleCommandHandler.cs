using Doctors.Application.Features.Doctors.Abstractions;
using Doctors.Domain.Utilities;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.RemoveWorkDaySchedule;

public sealed class RemoveWorkDayScheduleCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveWorkDayScheduleCommand>
{
    public async Task<Result> Handle(RemoveWorkDayScheduleCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        var result = doctor.RemoveWorkDay(request.DayOfWeek);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}