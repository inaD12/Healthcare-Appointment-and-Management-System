namespace Shared.Application.IntegrationEvents;

public sealed record DoctorRemovedUnavailabilityIntegrationEvent(
    string DoctorId,
    DateTime Start,
    DateTime End
);
