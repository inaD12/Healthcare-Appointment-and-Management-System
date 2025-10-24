namespace Shared.Application.IntegrationEvents;

public sealed record DoctorAddedUnavailabilityIntegrationEvent(
    string DoctorId,
    DateTime Start,
    DateTime End,
    string Reason
);
