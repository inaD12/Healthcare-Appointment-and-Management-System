using Appointments.Domain.Entities.Enums;
using Shared.Domain.Entities.Base;

namespace Appointments.Domain.Entities;

public class DoctorSchedule : BaseEntity
{
    public WeeklySchedule WeeklySchedule => new();
    
    private readonly List<AvailabilityException> _availabilityExceptions = new();
    public IReadOnlyCollection<AvailabilityException> AvailabilityExceptions => _availabilityExceptions;

    public DoctorSchedule(string id) { Id = id;}

    public bool IsSlotAvailable(DateTime start, DateTime end)
    {
        if (start >= end)
            return false;

        if (IsInUnavailablePeriod(start, end))
            return false;

        if (IsInExtraAvailability(start, end))
            return true;
        
        var startTime = TimeOnly.FromDateTime(start);
        var endTime = TimeOnly.FromDateTime(end);
        var day = start.DayOfWeek;

        return WeeklySchedule.IsWithinWorkHours(startTime, endTime, day);
    }
    
    public void AddOrUpdateWorkDay(
        DayOfWeek day, 
        IEnumerable<WorkTimeRange> workTimes)
    {
        WeeklySchedule.AddOrUpdateWorkDay(day, workTimes);
    }

    public void RemoveWorkDay(DayOfWeek day)
    {
        WeeklySchedule.RemoveWorkDay(day);
    }

    public void AddExtraAvailability(DateTime start, DateTime end, string reason)
    {
        _availabilityExceptions.Add(
            AvailabilityException.CreateExtraAvailability(start, end, reason));
    }

    public void AddUnavailability(DateTime start, DateTime end, string reason)
    {
        _availabilityExceptions.Add(
            AvailabilityException.CreateUnavailability(start, end, reason));
    }
    
    public void RemoveUnavailability(DateTime start, DateTime end)
    {
        var exceptions = FindOverlappingExceptions(
            start,
            end,
            ExceptionType.Unavailability);

        foreach (var exception in exceptions)
            _availabilityExceptions.Remove(exception);
    }

    public void RemoveExtraAvailability(DateTime start, DateTime end)
    {
        var exceptions = FindOverlappingExceptions(
            start,
            end,
            ExceptionType.ExtraAvailability);

        foreach (var exception in exceptions)
            _availabilityExceptions.Remove(exception);
    }
    
    private List<AvailabilityException> FindOverlappingExceptions(DateTime start, DateTime end, ExceptionType type)
    {
        var overlapping = _availabilityExceptions
            .Where(e => e.Type == type && e.Overlaps(start, end))
            .ToList();

        return overlapping;
    }
    
    private bool IsInUnavailablePeriod(DateTime start, DateTime end)
    {
        return AvailabilityExceptions.Any(p =>
            start < p.End && end > p.Start && p.Type == ExceptionType.Unavailability);
    }
    
    private bool IsInExtraAvailability(DateTime start, DateTime end)
    {
        return AvailabilityExceptions.Any(a => 
            start >= a.Start && end <= a.End && a.Type == ExceptionType.ExtraAvailability);
    }
}
