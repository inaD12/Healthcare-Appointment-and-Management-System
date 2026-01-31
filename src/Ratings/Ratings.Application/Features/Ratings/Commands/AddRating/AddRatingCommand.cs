using Ratings.Application.Features.Ratings.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Commands.AddRating;

public sealed record AddRatingCommand(
    string UserId,
    string AppointmentId,
    int Score,
    string? Comment) : ICommand<RatingCommandViewModel>;