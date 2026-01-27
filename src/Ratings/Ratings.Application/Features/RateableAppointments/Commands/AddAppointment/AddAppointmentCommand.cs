using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.RateableAppointments.Commands.AddAppointment;

public sealed record AddRatingCommand(
    string AppointmentId,
    int Score,
    string? Comment) : ICommand;