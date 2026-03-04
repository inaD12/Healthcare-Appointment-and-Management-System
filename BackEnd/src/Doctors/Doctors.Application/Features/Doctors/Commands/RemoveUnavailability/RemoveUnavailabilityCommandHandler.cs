using Doctors.Domain.Abstractions.Repositories;
using Doctors.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.RemoveUnavailability;

public sealed class RemoveDoctorUnavailabilityCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveUnavailabilityCommand>
{
    public async Task<Result> Handle(RemoveUnavailabilityCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        var result = doctor.RemoveUnavailability(request.Start,  request.End);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}