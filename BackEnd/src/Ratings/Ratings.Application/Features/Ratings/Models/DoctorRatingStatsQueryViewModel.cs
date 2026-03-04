namespace Ratings.Application.Features.Ratings.Models;

public sealed record DoctorRatingStatsQueryViewModel(
	string DoctorId,
	double AvarageRating,
	int RatingCount);