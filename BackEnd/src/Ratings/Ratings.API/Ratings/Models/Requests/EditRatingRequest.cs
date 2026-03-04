namespace Ratings.API.Ratings.Models.Requests;

public sealed record EditRatingRequest(
    int? Score,
    string? Comment);