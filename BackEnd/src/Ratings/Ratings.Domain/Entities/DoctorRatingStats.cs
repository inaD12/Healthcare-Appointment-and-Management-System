using Ratings.Domain.Events;
using Shared.Domain.Entities.Base;

namespace Ratings.Domain.Entities;

public sealed class DoctorRatingStats: BaseEntity
{
    public double AverageRating { get; private set; }
    public int RatingsCount { get; private set; }

    private DoctorRatingStats() { }

    private DoctorRatingStats(string doctorId)
    {
        Id = doctorId;
        AverageRating = 0;
        RatingsCount = 0;
    }

    public static DoctorRatingStats Create(string doctorId)
    {
        return new DoctorRatingStats(doctorId);
    }

    public void ApplyNewRating(int score)
    {
        var total = AverageRating * RatingsCount;
        RatingsCount++;
        AverageRating = (total + score) / RatingsCount;
        
        RaiseDomainEvent(new DoctorAverageRatingUpdatedDomainEvent(
            Id,
            AverageRating,
            RatingsCount)
        );
    }

    public void RemoveRating(int score)
    {
        if (RatingsCount <= 1)
        {
            RatingsCount = 0;
            AverageRating = 0;
            return;
        }

        var total = AverageRating * RatingsCount;
        RatingsCount--;
        AverageRating = (total - score) / RatingsCount;
        
        RaiseDomainEvent(new DoctorAverageRatingUpdatedDomainEvent(
            Id,
            AverageRating,
            RatingsCount)
        );
    }
}
