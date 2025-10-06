using Doctors.Domain.Responses;
using Shared.Domain.Results;

namespace Doctors.Domain.Entities;

public sealed class WeeklySchedule
{
    private readonly List<WorkDay> _workDays = new();

    public IReadOnlyCollection<WorkDay> WorkDays => _workDays.AsReadOnly();

    private WeeklySchedule() { }

    public static Result<WeeklySchedule> Create(IEnumerable<WorkDay> workDays)
    {
        var schedule = new WeeklySchedule();

        foreach (var day in workDays)
        {
            var res = schedule.AddWorkDay(day);

            if (res.IsFailure)
            {
                return Result<WeeklySchedule>.Failure(res.Response);
            }
        }

        return Result<WeeklySchedule>.Success(schedule);
    }

    public Result AddWorkDay(WorkDay workDay)
    {
        if (_workDays.Any(d => d.DayOfWeek == workDay.DayOfWeek))
            return Result.Failure(ResponseList.DuplicateWorkDay);

        _workDays.Add(workDay);
        return Result.Success();
    }

    public Result ChangeWorkDay(WorkDay workDay)
    {
        var currentWorkDay = _workDays.Find(d => d.DayOfWeek == workDay.DayOfWeek);
        if (currentWorkDay == null)
            return  Result.Failure(ResponseList.WorkDayNotExist);
        
        _workDays.Remove(currentWorkDay);
        _workDays.Add(workDay);
        return Result.Success();
    }

    public Result RemoveWorkDay(DayOfWeek dayOfWeek)
    {
        var currentWorkDay = _workDays.Find(d => d.DayOfWeek == dayOfWeek);
        if (currentWorkDay == null)
            return Result.Failure(ResponseList.WorkDayNotExist);
        
        _workDays.Remove(currentWorkDay);
        return Result.Success();
    }

    public bool IsWorkingAt(DateTime dateTimeUtc)
    {
        var localDay = dateTimeUtc.DayOfWeek;
        var localTime = dateTimeUtc.TimeOfDay;

        var workDay = _workDays.FirstOrDefault(d => d.DayOfWeek == localDay);
        return workDay?.IsWithinWorkingHours(localTime) ?? false;
    }
}
