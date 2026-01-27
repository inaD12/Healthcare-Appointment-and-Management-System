using Microsoft.EntityFrameworkCore;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.RateableAppointments.Commands.AddAppointment;

public sealed class AddAppointmentCommandHandler(
    IRateableAppointmentRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddAppointmentCommand>
{
    public async Task<Result> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
    {
        var entity = RateableAppointment.Create
        (
            request.AppointmentId,
            request.DoctorId,
            request.PatientId
        );

        try
        {
            repository.AddAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
        }
        
        return Result.Success();
    }
}
