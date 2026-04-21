using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.UnfinalizeEncounter;

public sealed class UnfinalizeEncounterCommandHandler(
    IEncounterCommandRepository encounterCommandRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UnfinalizeEncounterCommand>
{
    public async Task<Result> Handle(UnfinalizeEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterCommandRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        var result = encounter.UnFinalize(dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
