using Doctors.Application.Features.Doctors.Abstractions;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Application.Features.Specialities.Abstractions;
using Doctors.Domain.Entities;
using Doctors.Domain.Utilities;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.CreateDoctor;

public sealed class CreateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    ISpecialityRepository specialityRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateDoctorCommand, DoctorCommandViewModel>
{
    public async Task<Result<DoctorCommandViewModel>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var existingDoctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existingDoctor != null)
            return Result<DoctorCommandViewModel>.Failure(ResponseList.DoctorAlreadyExists);

        var found = new List<Speciality>();

        if (request.Specialities != null && request.Specialities.Any())
        {
            var result = await specialityRepository
                .GetByNamesAsync(request.Specialities, cancellationToken);

            found = result.Found;
            var missing = result.Missing;

            if (missing.Any())
            {
                return Result<DoctorCommandViewModel>.Failure(
                    ResponseList.SpecialityNotFound(missing));
            }
        }

        var doctorResult = Doctor.Create(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Bio,
            found
        );
        if (doctorResult.IsFailure)
            return Result<DoctorCommandViewModel>.Failure(doctorResult.Response);
        
        var doctor = doctorResult.Value!;

        await doctorRepository.AddAsync(doctor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<DoctorCommandViewModel>.Success(new DoctorCommandViewModel(doctor.Id));
    }
}
