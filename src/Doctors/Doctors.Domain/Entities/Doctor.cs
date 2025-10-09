using Doctors.Domain.Events;
using Doctors.Domain.Responses;
using Shared.Domain.Entities.Base;
using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Results;

namespace Doctors.Domain.Entities;

public sealed class Doctor : BaseEntity
{
    public string UserId { get; init; }
    public List<string> Specialities { get; private set; }
    public List<string> Locations { get; private set; }
    public string TimeZoneId { get; private set; }

    public WeeklySchedule WeeklySchedule { get; private set; }
    public List<DoctorAvailabilityException> AvailabilityExceptions { get; private set; }

    private Doctor(
        string userId,
        List<string> specialities,
        List<string> locations,
        string timeZoneId,
        WeeklySchedule weeklySchedule,
        List<DoctorAvailabilityException> availabilityExceptions)
    {
        UserId = userId;
        Specialities = specialities;
        Locations = locations;
        TimeZoneId = timeZoneId;
        WeeklySchedule = weeklySchedule;
        AvailabilityExceptions = availabilityExceptions;
    }

    public static Result<Doctor> Create(
        string userId,
        List<string> specialities,
        List<string> locations,
        string timeZoneId,
        WeeklySchedule? weeklySchedule = null,
        List<DoctorAvailabilityException>? availabilityExceptions = null)
    {
        if (!IsValidTimeZone(timeZoneId))
            return Result<Doctor>.Failure(ResponseList.InvalidTimezone);
        
        return  Result<Doctor>.Success(new Doctor(
            userId,
            specialities,
            locations,
            timeZoneId,
            weeklySchedule ?? WeeklySchedule.Create(null).Value!,
            availabilityExceptions ?? new List<DoctorAvailabilityException>()));
    }
    
    public Result AddUnavailability(DateTime start, DateTime end, string reason = "")
    {
        var result = AddAvailabilityException(DoctorAvailabilityException.Create(start, end, AvailabilityExceptionType.Unavailable, reason));
        
        if (result.IsSuccess)
        {
            RaiseDomainEvent(new DoctorAddedUnavailabilityDomainEvent(Id, start, end, reason));
        }
        
        return result;
    }

    public Result AddExtraAvailability(DateTime start, DateTime end, string reason = "")
    {
        var result = AddAvailabilityException(DoctorAvailabilityException.Create(start, end, AvailabilityExceptionType.ExtraAvailability, reason));

        if (result.IsSuccess)
        {
            RaiseDomainEvent(new DoctorAddedExtraAvailabilityDomainEvent(Id, start, end, reason));
        }
        
        return result;
    }

    public Result AddWorkDay(WorkDay workDay)
    {
        var result = WeeklySchedule.AddWorkDay(workDay);
        if (result.IsFailure)
            return result;

        RaiseDomainEvent(new WorkDayScheduleAddedDomainEvent(Id, workDay.DayOfWeek, workDay.WorkTimes ));
        return Result.Success();
    }

    public Result ChangeWorkDay(WorkDay workDay)
    {
        var result = WeeklySchedule.ChangeWorkDay(workDay);
        if (result.IsFailure)
            return result;

        RaiseDomainEvent(new WorkDayScheduleChangedDomainEvent(Id, workDay.DayOfWeek, workDay.WorkTimes ));
        return Result.Success();
    }

    public Result RemoveWorkDay(DayOfWeek dayOfWeek)
    {
        var result = WeeklySchedule.RemoveWorkDay(dayOfWeek);
        if (result.IsFailure)
            return result;

        RaiseDomainEvent(new WorkDayScheduleRemovedDomainEvent(Id, dayOfWeek));
        return Result.Success();
    }

    public bool IsAvailable(DateTimeRange range)
    {
        var exceptions = AvailabilityExceptions
            .Where(e => e.Overlaps(range))
            .ToList();

        if (exceptions.Any(e => e.Type == AvailabilityExceptionType.Unavailable))
            return false;

        bool worksNormally = WeeklySchedule.IsWorkingAt(range.Start);

        if (!worksNormally)
        {
            if (exceptions.Any(e => e.Type == AvailabilityExceptionType.ExtraAvailability))
                return true;
            return false;
        }

        return true;
    }
    
    private Result AddAvailabilityException(DoctorAvailabilityException exception)
    {
        if (AvailabilityExceptions.Any(e => e.Overlaps(exception.Range)))
            return Result.Failure(ResponseList.ExceptionOverlap);

        AvailabilityExceptions.Add(exception);
        return Result.Success();
    }
    
    private static bool IsValidTimeZone(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

