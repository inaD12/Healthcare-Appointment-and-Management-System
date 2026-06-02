using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveUnavailability;

public sealed record RemoveUnavailabilityCommand(
    string DoctorUserId,
    DateTime Start,
    DateTime End ) : ICommand;
