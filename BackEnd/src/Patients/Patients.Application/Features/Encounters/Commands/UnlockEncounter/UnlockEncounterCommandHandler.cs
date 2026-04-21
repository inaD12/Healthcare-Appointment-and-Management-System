using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.UnlockEncounter;

public sealed class UnlockEncounterCommandHandler(
    IEncounterCommandRepository encounterCommandRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UnlockEncounterCommand>
{
    public async Task<Result> Handle(UnlockEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterCommandRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        var result = encounter.Unlock(dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
