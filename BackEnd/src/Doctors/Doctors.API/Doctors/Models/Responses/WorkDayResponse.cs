namespace Doctors.API.Doctors.Models.Responses;

public sealed record WorkDayResponse(int DayOfWeek, List<WorkTimeRangeResponse> WorkTimes);