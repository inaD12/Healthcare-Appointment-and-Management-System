using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Commands.EditRating;

public sealed record EditRatingCommand(
    string UserId,
    string RatingId,
    int? Score,
    string? Comment) : ICommand;