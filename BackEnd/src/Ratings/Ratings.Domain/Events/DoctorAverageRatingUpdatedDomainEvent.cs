using Shared.Domain.Abstractions;

namespace Ratings.Domain.Events;

public sealed record DoctorAverageRatingUpdatedDomainEvent(
    string DoctorId,
    double NewAverageRating,
    int NewRatingsCount
) : IDomainEvent;
