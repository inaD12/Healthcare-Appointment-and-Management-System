using Ratings.Domain.Events;
using Shared.Application.IntegrationEvents;

namespace Ratings.Application.Features.Ratings.Mappers;

public static class EventMapper
{
    public static DoctorAverageRatingUpdatedIntegrationEvent ToIntegrationEvent(
        this DoctorAverageRatingUpdatedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.NewAverageRating,
            domainEvent.NewRatingsCount);
}