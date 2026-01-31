using Ratings.Application.Features.Ratings.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.Ratings.Queries.GetDoctorRatingStatsById;

public sealed record GetDoctorRatingStatsByIdQuery(string Id) : IQuery<DoctorRatingStatsQueryViewModel>;
