using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Patients.Application.Features.Patients.Commands.DeletePatient;

public sealed class DeletePatientCommandHandler(
    IPatientCommandRepository patientCommandRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeletePatientCommand>
{
    public async Task<Result> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientCommandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient == null)
            return Result.Failure(ResponseList.PatientNotFound);
        
        patientCommandRepository.Delete(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
