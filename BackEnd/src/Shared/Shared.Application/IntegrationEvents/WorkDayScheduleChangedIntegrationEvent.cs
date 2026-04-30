using Shared.Application.Dtos;

namespace Shared.Application.IntegrationEvents;

public sealed record WorkDayScheduleChangedIntegrationEvent(
    string DoctorUserId,
    DayOfWeek DayOfWeek,
    IReadOnlyCollection<TimeRangeDto> WorkTimes
);
