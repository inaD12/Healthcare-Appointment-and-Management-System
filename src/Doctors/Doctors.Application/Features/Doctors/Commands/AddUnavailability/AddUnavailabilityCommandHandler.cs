using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.AddUnavailability;

public sealed class AddDoctorUnavailabilityCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddUnavailabilityCommand>
{
    public async Task<Result> Handle(AddUnavailabilityCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        var result = doctor.AddUnavailability(request.Start,  request.End, request.Reason);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}