using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Entities;

namespace Ratings.Application.Features.Ratings.Mappers;

public static class QueryMapper
{
    public static RatingQueryViewModel ToQueryViewModel(
        this Rating rating)
        => new(
            rating.Id,
            rating.DoctorId,
            rating.PatientId,
            rating.AppointmentId,
            rating.Score,
            rating.CreatedAt,
            rating.Comment);
    
    public static DoctorRatingStatsQueryViewModel ToQueryViewModel(
        this DoctorRatingStats ratingStats)
        => new(
            ratingStats.Id,
            ratingStats.AverageRating,
            ratingStats.RatingsCount);
}