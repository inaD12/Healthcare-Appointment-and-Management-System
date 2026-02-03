namespace Ratings.API.Ratings.Models.Responses;

public sealed record DoctorRatingStatsQueryResponse(
    string DoctorId,
    double  AverageRating,
    int RatingsCount);