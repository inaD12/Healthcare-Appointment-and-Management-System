using Shared.Application.Dtos;

namespace Shared.Application.IntegrationEvents;

public sealed record WorkDayScheduleAddedIntegrationEvent(
    string DoctorUserId,
    DayOfWeek DayOfWeek,
    IReadOnlyCollection<TimeRangeDto> WorkTimes
);
