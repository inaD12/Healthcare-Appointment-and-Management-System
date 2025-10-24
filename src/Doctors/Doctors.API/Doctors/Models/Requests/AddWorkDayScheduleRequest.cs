namespace Doctors.API.Doctors.Models.Requests;

public sealed record AddWorkDayScheduleRequest(
    DayOfWeek DayOfWeek,
    List<WorkTimeRangeModel> WorkTimes );