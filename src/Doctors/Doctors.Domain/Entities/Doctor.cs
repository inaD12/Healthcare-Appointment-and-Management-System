using Doctors.Domain.Responses;
using Shared.Domain.Entities.Base;
using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Results;

namespace Doctors.Domain.Entities;

public sealed class Doctor : BaseEntity
{
    public string UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public List<string> Specialities { get; private set; }
    public List<string> Locations { get; private set; }
    public string TimeZoneId { get; private set; } = "UTC";

    public WeeklySchedule WeeklySchedule { get; private set; }
    public List<DoctorAvailabilityException> AvailabilityExceptions { get; private set; }

    private Doctor(
        string userId,
        string firstName,
        string lastName,
        List<string> specialities,
        List<string> locations,
        string timeZoneId,
        WeeklySchedule weeklySchedule,
        List<DoctorAvailabilityException> availabilityExceptions)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Specialities = specialities;
        Locations = locations;
        TimeZoneId = timeZoneId;
        WeeklySchedule = weeklySchedule;
        AvailabilityExceptions = availabilityExceptions;
    }

    public static Doctor Create(
        string userId,
        string firstName,
        string lastName,
        List<string> specialities,
        List<string> locations,
        string timeZoneId,
        WeeklySchedule weeklySchedule,
        List<DoctorAvailabilityException>? availabilityExceptions = null)
    {
        return new Doctor(
            userId,
            firstName,
            lastName,
            specialities,
            locations,
            timeZoneId,
            weeklySchedule,
            availabilityExceptions ?? new List<DoctorAvailabilityException>());
    }
    
    public Result AddUnavailability(DateTime start, DateTime end, string reason = "")
    {
        return AddAvailabilityException(DoctorAvailabilityException.Create(start, end, AvailabilityExceptionType.Unavailable, reason));
    }

    public Result AddExtraAvailability(DateTime start, DateTime end, string reason = "")
    {
        return AddAvailabilityException(DoctorAvailabilityException.Create(start, end, AvailabilityExceptionType.ExtraAvailability, reason));
    }
    
    public Result AddWorkDay(WorkDay workDay)
    {
        var result = WeeklySchedule.AddWorkDay(workDay);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }

    public Result ChangeWorkDay(WorkDay workDay)
    {
        var result = WeeklySchedule.ChangeWorkDay(workDay);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }

    public Result RemoveWorkDay(DayOfWeek dayOfWeek)
    {
        var result = WeeklySchedule.RemoveWorkDay(dayOfWeek);
        if (result.IsFailure)
            return result;

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
}

