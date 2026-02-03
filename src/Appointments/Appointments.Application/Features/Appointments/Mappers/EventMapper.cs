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
}