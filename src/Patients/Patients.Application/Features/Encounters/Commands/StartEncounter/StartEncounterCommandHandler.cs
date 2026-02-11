using Patients.Application.Features.Encounters.Mappers;
using Patients.Application.Features.Encounters.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.StartEncounter;

public sealed class StartEncounterCommandHandler(
    IEncounterRepository encounterRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<StartEncounterCommand, EncounterCommandViewModel>
{
    public async Task<Result<EncounterCommandViewModel>> Handle(StartEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByAppointmentId(request.AppointmentId, cancellationToken);
        if (encounter is not null)
            return Result<EncounterCommandViewModel>.Failure(ResponseList.EncounterAlreadyExists);
        
        //TODO: Check if appointment exists
        
       encounter = Encounter.Start(request.PatientId, request.DoctorId, request.AppointmentId, dateTimeProvider.UtcNow);
        
       await encounterRepository.AddAsync(encounter, cancellationToken);
       await unitOfWork.SaveChangesAsync(cancellationToken);

       var viewModel = encounter.ToCommandViewModel();
       return Result<EncounterCommandViewModel>.Success(viewModel);
    }
}
