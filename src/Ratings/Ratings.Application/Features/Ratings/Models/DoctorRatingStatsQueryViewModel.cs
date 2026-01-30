namespace Ratings.Application.Features.Ratings.Models;

public sealed record DoctorRatingStatsRatingQueryViewModel(
	string DoctorId,
	double AvarageRating,
	int RatingCount);