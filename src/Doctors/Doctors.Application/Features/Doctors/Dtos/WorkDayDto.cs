namespace Doctors.Application.Features.Doctors.Dtos;

public sealed record WorkDayDto(DayOfWeek DayOfWeek, List<WorkTimeRangeDto> WorkTimes);