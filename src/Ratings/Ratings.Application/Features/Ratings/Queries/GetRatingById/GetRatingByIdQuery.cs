using Ratings.Application.Features.Ratings.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Queries.GetRatingById;

public sealed record GetRatingByIdQuery(string Id) : IQuery<RatingQueryViewModel>;
