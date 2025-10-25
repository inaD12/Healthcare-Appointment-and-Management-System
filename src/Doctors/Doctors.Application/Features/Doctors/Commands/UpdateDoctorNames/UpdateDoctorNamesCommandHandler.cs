using Doctors.Application.Features.Doctors.Commands.UpdateDoctorInfo;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorNames;

public sealed class UpdateDoctorNamesCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateDoctorNamesCommand>
{
    public async Task<Result> Handle(UpdateDoctorNamesCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        doctor.UpdateNames(request.FirstName, request.LastName);
        
        doctorRepository.Update(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}