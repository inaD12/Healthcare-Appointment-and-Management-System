namespace Shared.Application.IntegrationEvents;

public sealed record WorkDayScheduleRemovedIntegrationEvent(
    string DoctorId,
    DayOfWeek DayOfWeek
);
