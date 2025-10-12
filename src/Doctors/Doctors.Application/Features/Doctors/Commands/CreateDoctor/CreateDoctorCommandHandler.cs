using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
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
        
        var existingSpecialities = await specialityRepository
            .GetByNamesAsync(request.Specialities, cancellationToken);

        var doctorSpecialities = new List<Speciality>();

        foreach (var name in request.Specialities)
        {
            var speciality = existingSpecialities.FirstOrDefault(s => s.Name == name)
                             ?? new Speciality(name);
            doctorSpecialities.Add(speciality);
        }
        
        var doctorResult = Doctor.Create(request.UserId, doctorSpecialities, request.TimeZoneId);
        if (doctorResult.IsFailure)
            return Result<DoctorCommandViewModel>.Failure(doctorResult.Response);
        
        var doctor = doctorResult.Value!;

        await doctorRepository.AddAsync(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<DoctorCommandViewModel>.Success(new DoctorCommandViewModel(doctor.Id));
    }
}
