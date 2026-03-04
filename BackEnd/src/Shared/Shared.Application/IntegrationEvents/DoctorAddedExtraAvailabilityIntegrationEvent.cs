namespace Shared.Application.IntegrationEvents;

public sealed record DoctorAddedExtraAvailabilityIntegrationEvent(
    string DoctorId,
    DateTime Start,
    DateTime End,
    string Reason
);
