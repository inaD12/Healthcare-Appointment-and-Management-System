using Patients.Application.Features.Encounters.Models;
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
    : ICommandHandler<AddNoteCommand, NoteCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<NoteCommandViewModel>> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Shared.Domain.Results.Result<NoteCommandViewModel>.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Shared.Domain.Results.Result<NoteCommandViewModel>.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.AddNote(request.Note, dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return Shared.Domain.Results.Result<NoteCommandViewModel>.Failure(result.Response);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Shared.Domain.Results.Result<NoteCommandViewModel>.Success(new NoteCommandViewModel(result.Value!));
    }
}
