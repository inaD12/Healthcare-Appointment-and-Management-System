namespace Ratings.API.Ratings;

internal static class Permissions
{
    internal const string AddRating = "ratings:create";
    internal const string EditRating = "ratings:update";
    internal const string RemoveRating = "ratings:delete";
    internal const string GetRating = "ratings:read";
    internal const string GetRatingStats = "ratingStats:read";
}