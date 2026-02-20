using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Encounters.Commands.AddDiagnosis;

public sealed class AddDiagnosisCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddDiagnosisCommand>
{
    public async Task<Result> Handle(AddDiagnosisCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Result.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Result.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.AddDiagnosis(request.IcdCode, request.Description);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
