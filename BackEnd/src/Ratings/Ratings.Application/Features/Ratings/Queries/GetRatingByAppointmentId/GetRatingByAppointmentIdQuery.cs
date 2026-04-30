using Ratings.Application.Features.Ratings.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Queries.GetRatingByAppointmentId;

public sealed record GetRatingByAppointmentIdQuery(string AppointmentId) : IQuery<RatingQueryViewModel>;
