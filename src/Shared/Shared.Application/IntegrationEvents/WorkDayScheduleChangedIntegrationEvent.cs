using Shared.Application.Dtos;

namespace Shared.Application.IntegrationEvents;

public sealed record WorkDayScheduleChangedIntegrationEvent(
    string DoctorId,
    DayOfWeek DayOfWeek,
    IReadOnlyCollection<TimeRangeDto> WorkTimes
);
