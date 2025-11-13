using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Serilog;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Responses;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.CreateDoctor;

public sealed class CreateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    ISpecialityRepository specialityRepository,
    INamesService namesService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateDoctorCommand, DoctorCommandViewModel>
{
    public async Task<Result<DoctorCommandViewModel>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var existingDoctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existingDoctor != null)
            return Result<DoctorCommandViewModel>.Failure(ResponseList.DoctorAlreadyExists);
        
        var namesResult = await namesService.GetUserNamesAsync(request.UserId);
        if (namesResult.IsFailure)
        {
            Log.Error($"User id {request.UserId} from JWT does not return names");
            return Result<DoctorCommandViewModel>.Failure(SharedResponses.InternalError);
        }
        
        var (found, missing) = await specialityRepository
            .GetByNamesAsync(request.Specialities, cancellationToken);

        if (missing.Any())
        {
            return Result<DoctorCommandViewModel>.Failure(ResponseList.SpecialityNotFound(missing));
        }
        
        var names = namesResult.Value!;
        
        var doctorResult = Doctor.Create(request.UserId, names.FirstName,names.LastName, request.Bio, found, request.TimeZoneId);
        if (doctorResult.IsFailure)
            return Result<DoctorCommandViewModel>.Failure(doctorResult.Response);
        
        var doctor = doctorResult.Value!;

        await doctorRepository.AddAsync(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<DoctorCommandViewModel>.Success(new DoctorCommandViewModel(doctor.Id));
    }
}
