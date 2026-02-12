using Patients.Application.Features.Encounters.Commands.RemoveDiagnosis;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Encounters.Commands.RemovePrescription;

public sealed class RemovePrescriptionCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemovePrescriptionCommand>
{
    public async Task<Result> Handle(RemovePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        var result = encounter.RemovePrescription(request.PrescriptionId);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
