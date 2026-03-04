namespace Shared.Application.IntegrationEvents;

public sealed record AppointmentRescheduledIntegrationEvent(
    string AppointmentId
);
