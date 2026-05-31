using Patients.Application.Features.Encounters.Abstractions;
using Patients.Application.Features.Encounters.Models;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.AddDiagnosis;

public sealed class AddDiagnosisCommandHandler(
    IEncounterCommandRepository encounterCommandRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddDiagnosisCommand, DiagnosisCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<DiagnosisCommandViewModel>> Handle(AddDiagnosisCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterCommandRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Shared.Domain.Results.Result<DiagnosisCommandViewModel>.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Shared.Domain.Results.Result<DiagnosisCommandViewModel>.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.AddDiagnosis(request.IcdCode, request.Description, dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return Shared.Domain.Results.Result<DiagnosisCommandViewModel>.Failure(result.Response);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Shared.Domain.Results.Result<DiagnosisCommandViewModel>.Success(new DiagnosisCommandViewModel(result.Value!));
    }
}
