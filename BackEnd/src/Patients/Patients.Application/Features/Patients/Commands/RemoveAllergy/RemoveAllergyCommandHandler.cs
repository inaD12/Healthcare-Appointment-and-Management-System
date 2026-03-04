using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Patients.Commands.RemoveAllergy;

public sealed class RemoveAllergyCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveAllergyCommand>
{
    public async Task<Result> Handle(RemoveAllergyCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Result.Failure(ResponseList.PatientNotFound);
        
        var result = patient.RemoveAllergy(request.AllergyId);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
