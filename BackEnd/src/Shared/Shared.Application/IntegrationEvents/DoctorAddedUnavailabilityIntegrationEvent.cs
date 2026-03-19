namespace Shared.Application.IntegrationEvents;

public sealed record DoctorAddedUnavailabilityIntegrationEvent(
    string DoctorUserId,
    DateTime Start,
    DateTime End,
    string Reason
);
