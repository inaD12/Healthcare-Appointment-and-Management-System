using FluentValidation.Results;
using Ratings.Domain.Events;
using Shared.Domain.Entities.Base;
using Shared.Domain.Exceptions;

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
        if (score < 1 || score > 5)
            throw new HamsValidationException(new[]
            {
                new ValidationFailure(
                    "Rating", "Rating score must be between 1 and 5.")
            });
        
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
        if (score < 1 || score > 5)
            throw new HamsValidationException(new[]
            {
                new ValidationFailure(
                    "Rating", "Rating score must be between 1 and 5.")
            });
        
        if (RatingsCount <= 1)
        {
            RatingsCount = 0;
            AverageRating = 0;

            RaiseDomainEvent(new DoctorAverageRatingUpdatedDomainEvent(
                Id,
                AverageRating,
                RatingsCount));

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
