using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.AddNote;

public sealed class AddNoteCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddNoteCommand>
{
    public async Task<Result> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Result.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.AddNote(request.Note, dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
