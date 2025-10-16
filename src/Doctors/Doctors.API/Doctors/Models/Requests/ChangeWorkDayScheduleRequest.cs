namespace Doctors.API.Doctors.Models.Requests;

public sealed record ChangeWorkDayScheduleRequest(
    DayOfWeek DayOfWeek,
    List<WorkTimeRangeModel> WorkTimes );