namespace Shared.Application.IntegrationEvents;

public sealed record WorkDayScheduleRemovedIntegrationEvent(
    string DoctorUserId,
    DayOfWeek DayOfWeek
);
