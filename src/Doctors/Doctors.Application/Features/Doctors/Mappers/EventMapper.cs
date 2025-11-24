using Doctors.Domain.Entities;
using Doctors.Domain.Events;
using Shared.Application.Dtos;
using Shared.Application.IntegrationEvents;

namespace Doctors.Application.Features.Doctors.Mappers;

public static class EventMapper
{
    public static DoctorAddedExtraAvailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorAddedExtraAvailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.Start,
            domainEvent.End,
            domainEvent.Reason);
    
    public static DoctorAddedUnavailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorAddedUnavailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.Start,
            domainEvent.End,
            domainEvent.Reason);
    
    public static DoctorRemovedExtraAvailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorRemovedExtraAvailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.Start,
            domainEvent.End);
    
    public static DoctorRemovedUnavailabilityIntegrationEvent ToIntegrationEvent(
        this DoctorRemovedUnavailabilityDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.Start,
            domainEvent.End);
    
    public static WorkDayScheduleAddedIntegrationEvent ToIntegrationEvent(
        this WorkDayScheduleAddedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.DayOfWeek,
            domainEvent.WorkTimes.Select(range => range.ToTimeSpanRangeDto()).ToList());
    
    public static WorkDayScheduleChangedIntegrationEvent ToIntegrationEvent(
        this WorkDayScheduleChangedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.DayOfWeek,
            domainEvent.WorkTimes.Select(range => range.ToTimeSpanRangeDto()).ToList());
    
    public static WorkDayScheduleRemovedIntegrationEvent ToIntegrationEvent(
        this WorkDayScheduleRemovedDomainEvent domainEvent)
        => new(
            domainEvent.DoctorId,
            domainEvent.DayOfWeek);
    
    public static TimeSpanRangeDto ToTimeSpanRangeDto(
        this WorkTimeRange workTimeRange)
        => new(
            workTimeRange.Start,
            workTimeRange.End);
}