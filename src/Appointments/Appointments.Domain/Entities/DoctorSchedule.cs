namespace Appointments.Domain.Entities;

public class DoctorSchedule
{
    public string DoctorId { get; set; }

    public Dictionary<DayOfWeek, List<WorkTimeRange>> WorkDaySchedules { get; set; } = new();

    public List<ExtraAvailability> ExtraAvailabilities { get; set; } = new();

    public List<Unavailability> Unavailabilities { get; set; } = new();
}

public class ExtraAvailability
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class Unavailability
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class WorkTimeRange
{
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}

