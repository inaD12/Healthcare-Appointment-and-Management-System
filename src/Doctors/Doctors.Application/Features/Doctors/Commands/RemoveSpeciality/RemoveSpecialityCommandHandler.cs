using Doctors.Domain.Abstractions.Repositories;
using Doctors.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
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
        
        await doctorRepository.AddAsync(doctor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
