using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddUnavailability;

public sealed record AddUnavailabilityCommand(
    string DoctorUserId,
    DateTime Start,
    DateTime End,
    string Reason ) : ICommand;
