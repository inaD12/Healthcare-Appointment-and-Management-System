namespace Appointments.Domain.Entities;

public class WeeklySchedule
{
    private readonly List<WorkDaySchedule> _workDays = new();

    public IReadOnlyCollection<WorkDaySchedule> WorkDays => _workDays;

    public void AddOrUpdateWorkDay(
        DayOfWeek day, 
        IEnumerable<WorkTimeRange> workTimes)
    {
        var sortedRanges = workTimes
            .Select(r => WorkTimeRange.Create(r.Start, r.End))
            .OrderBy(r => r.Start)
            .ToList();

        var existing = _workDays.FirstOrDefault(x => x.DayOfWeek == day);

        if (existing != null)
        {
            existing.UpdateTimes(sortedRanges);
        }
        else
        {
            _workDays.Add(new WorkDaySchedule(day, sortedRanges));
        }
    }

    public void RemoveWorkDay(DayOfWeek day)
    {
        var daySchedule = _workDays.FirstOrDefault(x => x.DayOfWeek == day);
        if (daySchedule != null)
            _workDays.Remove(daySchedule);
    }
}
