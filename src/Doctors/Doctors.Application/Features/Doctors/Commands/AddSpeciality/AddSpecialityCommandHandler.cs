using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.AddSpeciality;

public sealed class AddSpecialityCommandHandler(
    IDoctorRepository doctorRepository,
    ISpecialityRepository specialityRepository,
    IEmbeddingClient  embeddingClient,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddSpecialityCommand>
{
    public async Task<Result> Handle(AddSpecialityCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor is null)
            return Result.Failure(ResponseList.DoctorNotFound);

        var speciality = await specialityRepository.GetByNameAsync(request.Speciality, cancellationToken);
        if (speciality is null)
            return Result.Failure(ResponseList.SpecialityNotBelongDoctor);
        
        var result = doctor.AddSpeciality(speciality);
        if (result.IsFailure)
            return result;
        
        await doctorRepository.AddAsync(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
