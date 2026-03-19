namespace Shared.Application.IntegrationEvents;

public sealed record DoctorRemovedUnavailabilityIntegrationEvent(
    string DoctorUserId,
    DateTime Start,
    DateTime End
);
