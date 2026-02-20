using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Encounters.Commands.PrescribeMedication;

public sealed class PrescribeMedicationCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PrescribeMedicationCommand>
{
    public async Task<Result> Handle(PrescribeMedicationCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Result.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.PrescribeMedication(request.Name, request.Dosage, request.Instructions);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
