using Patients.Application.Features.Patients.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.AddAllergy;

public sealed class AddAllergyCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddAllergyCommand, AllergyCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<AllergyCommandViewModel>> Handle(AddAllergyCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Shared.Domain.Results.Result<AllergyCommandViewModel>.Failure(ResponseList.PatientNotFound);
        
        var result = patient.AddAllergy(request.Substance, request.Substance);
        if (result.IsFailure)
            return Shared.Domain.Results.Result<AllergyCommandViewModel>.Failure(result.Response);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Shared.Domain.Results.Result<AllergyCommandViewModel>.Success(new AllergyCommandViewModel(result.Value!));
    }
}
