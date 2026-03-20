namespace Shared.Application.IntegrationEvents;

public sealed record DoctorAverageRatingUpdatedIntegrationEvent(
    string DoctorId,
    double NewAverageRating,
    int NewRatingsCount
);
