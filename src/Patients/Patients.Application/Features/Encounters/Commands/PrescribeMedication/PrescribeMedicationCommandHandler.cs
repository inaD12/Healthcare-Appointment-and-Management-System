using Patients.Application.Features.Encounters.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.PrescribeMedication;

public sealed class PrescribeMedicationCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PrescribeMedicationCommand, PrescriptionCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<PrescriptionCommandViewModel>> Handle(PrescribeMedicationCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByIdAsync(request.EncounterId, cancellationToken);
        if (encounter is  null)
            return Shared.Domain.Results.Result<PrescriptionCommandViewModel>.Failure(ResponseList.EncounterNotFound);
        
        if(encounter.DoctorId != request.UserId)
            return Shared.Domain.Results.Result<PrescriptionCommandViewModel>.Failure(ResponseList.NotTheDoctor);
        
        var result = encounter.PrescribeMedication(request.Name, request.Dosage, request.Instructions);
        if (result.IsFailure)
            return Shared.Domain.Results.Result<PrescriptionCommandViewModel>.Failure(result.Response);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Shared.Domain.Results.Result<PrescriptionCommandViewModel>.Success(new PrescriptionCommandViewModel(result.Value!));
    }
}
