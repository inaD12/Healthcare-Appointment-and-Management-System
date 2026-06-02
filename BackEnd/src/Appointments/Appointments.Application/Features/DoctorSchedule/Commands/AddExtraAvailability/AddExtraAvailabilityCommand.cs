using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddExtraAvailability;

public sealed record AddExtraAvailabilityCommand(
    string DoctorUserId,
    DateTime Start,
    DateTime End,
    string Reason ) : ICommand;
