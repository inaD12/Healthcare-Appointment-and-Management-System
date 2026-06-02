using Doctors.Application.Features.Doctors.Abstractions;
using Doctors.Domain.Utilities;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.AddExtraAvailability;

public sealed class AddExtraAvailabilityCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddExtraAvailabilityCommand>
{
    public async Task<Result> Handle(AddExtraAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        var result = doctor.AddExtraAvailability(request.Start,  request.End, request.Reason);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}