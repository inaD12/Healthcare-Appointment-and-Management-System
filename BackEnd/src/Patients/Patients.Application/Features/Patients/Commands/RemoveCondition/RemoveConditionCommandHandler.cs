using Patients.Application.Features.Patients.Commands.RemoveAllergy;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Patients.Commands.RemoveCondition;

public sealed class RemoveConditionCommandHandler(
    IPatientCommandRepository patientCommandRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveConditionCommand>
{
    public async Task<Result> Handle(RemoveConditionCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientCommandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Result.Failure(ResponseList.PatientNotFound);
        
        var result = patient.RemoveCondition(request.ConditionId);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
