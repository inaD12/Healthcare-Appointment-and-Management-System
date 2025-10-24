using Shared.Application.Dtos;

namespace Shared.Application.IntegrationEvents;

public sealed record WorkDayScheduleAddedIntegrationEvent(
    string DoctorId,
    DayOfWeek DayOfWeek,
    IReadOnlyCollection<TimeSpanRangeDto> WorkTimes
);
