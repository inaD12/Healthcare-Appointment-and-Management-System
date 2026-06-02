using Doctors.Application.Features.Doctors.Abstractions;
using Doctors.Domain.Utilities;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.RemoveSpeciality;

public sealed class RemoveSpecialityCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveSpecialityCommand>
{
    public async Task<Result> Handle(RemoveSpecialityCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor is null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        var result = doctor.RemoveSpeciality(request.Speciality);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
