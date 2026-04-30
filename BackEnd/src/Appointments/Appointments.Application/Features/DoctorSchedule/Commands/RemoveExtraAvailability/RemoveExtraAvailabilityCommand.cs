using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveExtraAvailability;

public sealed record RemoveExtraAvailabilityCommand(
    string DoctorUserId,
    DateTime Start,
    DateTime End ) : ICommand;
