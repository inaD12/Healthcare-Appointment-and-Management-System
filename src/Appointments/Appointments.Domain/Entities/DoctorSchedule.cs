using Appointments.Domain.Entities.Enums;
using Shared.Domain.Entities.Base;
using Shared.Domain.Results;

namespace Appointments.Domain.Entities;

public class DoctorSchedule : BaseEntity
{
    public WeeklySchedule WeeklySchedule => new();
    public List<AvailabilityException> AvailabilityExceptions => new();

    public DoctorSchedule() { }

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
        AvailabilityExceptions.Add(
            AvailabilityException.CreateExtraAvailability(start, end, reason));
    }

    public void AddUnavailability(DateTime start, DateTime end, string reason)
    {
        AvailabilityExceptions.Add(
            AvailabilityException.CreateUnavailability(start, end, reason));
    }
    
    public void RemoveUnavailability(DateTime start, DateTime end)
    {
        var exceptions = FindOverlappingExceptions(
            start,
            end,
            ExceptionType.Unavailability);

        foreach (var exception in exceptions)
            AvailabilityExceptions.Remove(exception);
    }

    public void RemoveExtraAvailability(DateTime start, DateTime end)
    {
        var exceptions = FindOverlappingExceptions(
            start,
            end,
            ExceptionType.ExtraAvailability);

        foreach (var exception in exceptions)
            AvailabilityExceptions.Remove(exception);
    }
    
    private List<AvailabilityException> FindOverlappingExceptions(DateTime start, DateTime end, ExceptionType type)
    {
        var overlapping = AvailabilityExceptions
            .Where(e => e.Type == type && e.Overlaps(start, end))
            .ToList();

        return overlapping;
    }
}
