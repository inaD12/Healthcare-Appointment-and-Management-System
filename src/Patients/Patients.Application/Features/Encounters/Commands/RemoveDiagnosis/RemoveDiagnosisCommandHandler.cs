using Patients.Application.Features.Encounters.Commands.RemoveNote;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Encounters.Commands.RemoveDiagnosis;

public sealed class RemoveDiagnosisCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveDiagnosisCommand>
{
    public async Task<Result> Handle(RemoveDiagnosisCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        var result = encounter.RemoveDiagnosis(request.DiagnosisId);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
