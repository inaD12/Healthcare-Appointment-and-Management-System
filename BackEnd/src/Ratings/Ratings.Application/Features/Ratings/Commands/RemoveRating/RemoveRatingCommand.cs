using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Commands.RemoveRating;

public sealed record RemoveRatingCommand(
    string UserId,
    string RatingId) : ICommand;