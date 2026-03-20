using Ratings.Application.Features.Ratings.Models;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;

namespace Ratings.Application.Features.Ratings.Queries.GetAllRatingsByDoctor;

public sealed record GetAllRatingsByDoctorQuery(
	string DoctorId,
	string? PatientId,
	string? AppointmentId,
	int? MinScore,
	int? MaxScore,
	SortOrder SortOrder,
	string SortPropertyName,
	int Page,
	int PageSize) : IQuery<RatingPaginatedQueryViewModel>;
