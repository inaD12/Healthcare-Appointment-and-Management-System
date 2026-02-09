using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Patients.Commands.AddAllergy;

public sealed class AddAllergyCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddAllergyCommand>
{
    public async Task<Result> Handle(AddAllergyCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Result.Failure(ResponseList.PatientNotFound);
        
        var result = patient.AddAllergy(request.Substance, request.Substance);
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
