using Doctors.Application.Features.Doctors.Abstractions;
using Doctors.Domain.Utilities;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorInfo;

public sealed class UpdateDoctorInfoCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateDoctorInfoCommand>
{
    public async Task<Result> Handle(UpdateDoctorInfoCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        doctor.UpdateProfile(request.NewBio);
        
        doctorRepository.Update(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}