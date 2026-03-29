using Patients.Application.Features.Encounters.Mappers;
using Patients.Application.Features.Encounters.Models;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Infrastructure.Clock;

namespace Patients.Application.Features.Encounters.Commands.StartEncounter;

public sealed class StartEncounterCommandHandler(
    IEncounterRepository encounterRepository,
    IAppointmentReadRepository  appointmentRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<StartEncounterCommand, EncounterCommandViewModel>
{
    public async Task<Shared.Domain.Results.Result<EncounterCommandViewModel>> Handle(StartEncounterCommand request, CancellationToken cancellationToken)
    {
        var encounter = await encounterRepository.GetByAppointmentId(request.AppointmentId, cancellationToken);
        if (encounter is not null)
            return Shared.Domain.Results.Result<EncounterCommandViewModel>.Failure(ResponseList.EncounterAlreadyExists);
        
        var appointment = await appointmentRepository.GetAsync(request.AppointmentId, cancellationToken);
        if (appointment is null)
            return Shared.Domain.Results.Result<EncounterCommandViewModel>.Failure(ResponseList.AppointmentNotFound);
        
        encounter = Encounter.Start(appointment.PatientId, appointment.DoctorId, request.AppointmentId, dateTimeProvider.UtcNow);
        
       await encounterRepository.AddAsync(encounter, cancellationToken);
       await unitOfWork.SaveChangesAsync(cancellationToken);

       var viewModel = encounter.ToCommandViewModel();
       return Shared.Domain.Results.Result<EncounterCommandViewModel>.Success(viewModel);
    }
}
