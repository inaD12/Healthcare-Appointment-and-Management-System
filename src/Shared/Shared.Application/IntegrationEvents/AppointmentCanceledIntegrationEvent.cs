namespace Shared.Application.IntegrationEvents;

public sealed record AppointmentCanceledIntegrationEvent(
    string AppointmentId
);
