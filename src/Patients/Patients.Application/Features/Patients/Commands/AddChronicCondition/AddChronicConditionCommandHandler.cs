using Patients.Application.Features.Patients.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.AddChronicCondition;

public sealed class AAddChronicConditionCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddChronicConditionCommand, ConditionCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<ConditionCommandViewModel>> Handle(AddChronicConditionCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Shared.Domain.Results.Result<ConditionCommandViewModel>.Failure(ResponseList.PatientNotFound);
        
        var result = patient.AddChronicCondition(request.Name);
        if (result.IsFailure)
            return Shared.Domain.Results.Result<ConditionCommandViewModel>.Failure(result.Response);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Shared.Domain.Results.Result<ConditionCommandViewModel>.Success(new ConditionCommandViewModel(result.Value!));
    }
}
