using Appointments.Domain.Events;
using Shared.Application.IntegrationEvents;

namespace Appointments.Application.Features.Appointments.Mappers;

public static class EventMapper
{
    public static AppointmentCompletedIntegrationEvent ToIntegrationEvent(
        this AppointmentCompletedDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId,
            domainEvent.DoctorId,
            domainEvent.PatientId);
    
    public static AppointmentCreatedIntegrationEvent ToIntegrationEvent(
        this AppointmentCreatedDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId,
            domainEvent.DoctorId,
            domainEvent.PatientId,
            domainEvent.Start,
            domainEvent.End);
    
    public static AppointmentCanceledIntegrationEvent ToIntegrationEvent(
        this AppointmentCanceledDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId);
    
    public static AppointmentRescheduledIntegrationEvent ToIntegrationEvent(
        this AppointmentRescheduledDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId);
}