using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveExtraAvailability;

public sealed record RemoveExtraAvailabilityCommand(
    string DoctorId,
    DateTime Start,
    DateTime End ) : ICommand;
