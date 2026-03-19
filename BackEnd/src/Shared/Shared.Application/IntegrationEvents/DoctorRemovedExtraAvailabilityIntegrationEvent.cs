namespace Shared.Application.IntegrationEvents;

public sealed record DoctorRemovedExtraAvailabilityIntegrationEvent(
    string DoctorUserId,
    DateTime Start,
    DateTime End
);
