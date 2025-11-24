namespace Appointments.Domain.Entities;

public class DoctorSchedule
{
    public string DoctorId { get; set; }

    public WeeklySchedule WeeklySchedule { get; set; } = new();

    public List<AvailabilityException> AvailabilityExceptions { get; set; } = new();
}

public class WeeklySchedule
{
    public List<WorkDaySchedule> WorkDays { get; set; } = new();
}

public class WorkDaySchedule
{
    public DayOfWeek DayOfWeek { get; set; }
    public List<WorkTimeRange> WorkTimes { get; set; } = new();
}

public class WorkTimeRange
{
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}

public class AvailabilityException
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Reason { get; set; } = string.Empty;
    public ExceptionType Type { get; set; }
}

public enum ExceptionType
{
    ExtraAvailability,
    Unavailability
}

