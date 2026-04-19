using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.FinalizeEncounter;

public sealed class FinalizeEncounterCommandHandler(
    IEncounterCommandRepository encounterCommandRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<FinalizeEncounterCommand>
{
    public async Task<Result> Handle(FinalizeEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterCommandRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Result.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.Finalize(dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
