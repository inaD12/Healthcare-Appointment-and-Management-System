using Patients.Application.Features.Encounters.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.AddAddendum;

public sealed class AddAddendumCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddAddendumCommand, AddendumCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<AddendumCommandViewModel>> Handle(AddAddendumCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Shared.Domain.Results.Result<AddendumCommandViewModel>.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Shared.Domain.Results.Result<AddendumCommandViewModel>.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.AddAddendum(request.Note, dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return Shared.Domain.Results.Result<AddendumCommandViewModel>.Failure(result.Response);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Shared.Domain.Results.Result<AddendumCommandViewModel>.Success(new AddendumCommandViewModel(result.Value!));
    }
}
