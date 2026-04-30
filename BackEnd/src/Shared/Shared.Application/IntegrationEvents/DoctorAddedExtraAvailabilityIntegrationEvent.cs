namespace Shared.Application.IntegrationEvents;

public sealed record DoctorAddedExtraAvailabilityIntegrationEvent(
    string DoctorUserId,
    DateTime Start,
    DateTime End,
    string Reason
);
