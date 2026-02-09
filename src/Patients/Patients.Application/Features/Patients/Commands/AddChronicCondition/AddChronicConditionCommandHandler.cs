using Patients.Application.Features.Patients.Commands.AddAllergy;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Patients.Commands.AddChronicCondition;

public sealed class AAddChronicConditionCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddChronicConditionCommand>
{
    public async Task<Result> Handle(AddChronicConditionCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Result.Failure(ResponseList.PatientNotFound);
        
        var result = patient.AddChronicCondition(request.Name);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
