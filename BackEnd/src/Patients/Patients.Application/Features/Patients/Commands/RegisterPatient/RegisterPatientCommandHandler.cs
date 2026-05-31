using Patients.Application.Features.Patients.Abstractions;
using Patients.Application.Features.Patients.Mappers;
using Patients.Application.Features.Patients.Models;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.RegisterPatient;

public sealed class RegisterPatientCommandHandler(
    IPatientCommandRepository patientCommandRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterPatientCommand, PatientCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<PatientCommandViewModel>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {
       var patient = await patientCommandRepository.GetByIdAsync(request.UserId, cancellationToken);
       if (patient is not null)
       {
           return Shared.Domain.Results.Result<PatientCommandViewModel>.Failure(ResponseList.UserIdAlreadyInUse); 
       }

       patient = Patient.Register(request.UserId, request.FirstName, request.LastName, request.BirthDate);
       
       await patientCommandRepository.AddAsync(patient, cancellationToken);
       await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var patientCommandViewModel = patient.ToCommandViewModel();
        return Shared.Domain.Results.Result<PatientCommandViewModel>.Success(patientCommandViewModel);
    }
}
