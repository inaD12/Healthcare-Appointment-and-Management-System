using Doctors.Domain.Entities;
using Doctors.Domain.Events;
using Shared.Application.Dtos;
using Shared.Application.IntegrationEvents;

namespace Doctors.Infrastructure.Features.Mappers;

public static class EventMapper
{
    public static DoctorAddedExtraAvailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorAddedExtraAvailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.Start,
            domainEvent.End,
            domainEvent.Reason);
    
    public static DoctorAddedUnavailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorAddedUnavailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.Start,
            domainEvent.End,
            domainEvent.Reason);
    
    public static DoctorRemovedExtraAvailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorRemovedExtraAvailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.Start,
            domainEvent.End);
    
    public static DoctorRemovedUnavailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorRemovedUnavailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.Start,
            domainEvent.End);
    
    public static WorkDayScheduleAddedIntegrationEvent ToIntegrationEvent(
        this WorkDayScheduleAddedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.DayOfWeek,
            domainEvent.WorkTimes.Select(range => range.ToTimeRangeRangeDto()).ToList());
    
    public static WorkDayScheduleChangedIntegrationEvent ToIntegrationEvent(
        this WorkDayScheduleChangedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.DayOfWeek,
            domainEvent.WorkTimes.Select(range => range.ToTimeRangeRangeDto()).ToList());
    
    public static WorkDayScheduleRemovedIntegrationEvent ToIntegrationEvent(
        this WorkDayScheduleRemovedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorUserId,
            domainEvent.DayOfWeek);
    
    public static TimeRangeDto ToTimeRangeRangeDto(
        this WorkTimeRange workTimeRange)
        => new(
            workTimeRange.Start,
            workTimeRange.End);
}