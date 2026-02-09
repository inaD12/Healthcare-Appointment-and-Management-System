using Patients.Application.Features.Patients.Mappers;
using Patients.Application.Features.Patients.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Patients.Commands.RegisterPatient;

public sealed class RegisterPatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterPatientCommand, PatientCommandViewModel>
{
    public async Task<Result<PatientCommandViewModel>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {
       var patient = await patientRepository.GetByIdAsync(request.UserId, cancellationToken);
       if (patient is not null)
       {
           return Result<PatientCommandViewModel>.Failure(ResponseList.UserIdAlreadyInUse); 
       }

       patient = Patient.Register(request.UserId, request.FirstName, request.LastName, request.BirthDate);
       
       await patientRepository.AddAsync(patient, cancellationToken);
       await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var patientCommandViewModel = patient.ToCommandViewModel();
        return Result<PatientCommandViewModel>.Success(patientCommandViewModel);
    }
}
