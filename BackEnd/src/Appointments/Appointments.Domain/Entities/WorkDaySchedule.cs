namespace Appointments.Domain.Entities;

public class WorkDaySchedule
{
    private readonly List<WorkTimeRange> _workTimes = new();

    public DayOfWeek DayOfWeek { get; private set; }
    public IReadOnlyCollection<WorkTimeRange> WorkTimes => _workTimes;

    private WorkDaySchedule() { }
    
    public WorkDaySchedule(DayOfWeek day, IEnumerable<WorkTimeRange> workTimes)
    {
        DayOfWeek = day;
        _workTimes = workTimes.ToList();
    }

    public void UpdateTimes(IEnumerable<WorkTimeRange> workTimes)
    {
        var newList = workTimes.ToList();
        _workTimes.Clear();
        _workTimes.AddRange(newList);
    }
}
