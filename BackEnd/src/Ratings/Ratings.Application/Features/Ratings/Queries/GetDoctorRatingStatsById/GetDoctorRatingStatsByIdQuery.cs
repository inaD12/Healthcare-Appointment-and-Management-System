using Ratings.Application.Features.Ratings.Models;
using Shared.Application.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Queries.GetDoctorRatingStatsById;

public sealed record GetDoctorRatingStatsByIdQuery(string Id) : IQuery<DoctorRatingStatsQueryViewModel>;
