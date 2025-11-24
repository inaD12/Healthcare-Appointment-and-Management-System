namespace Shared.Application.IntegrationEvents;

public sealed record DoctorRemovedExtraAvailabilityIntegrationEvent(
    string DoctorId,
    DateTime Start,
    DateTime End
);
