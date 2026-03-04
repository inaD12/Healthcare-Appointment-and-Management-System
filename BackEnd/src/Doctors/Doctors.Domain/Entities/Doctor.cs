using Doctors.Domain.Events;
using Doctors.Domain.Utilities;
using Shared.Domain.Entities.Base;
using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Results;

namespace Doctors.Domain.Entities;

public sealed class Doctor : BaseEntity
{
    public string UserId { get; init; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Bio { get; private set; }
    public string TimeZoneId { get; private set; }
    public List<Speciality> Specialities { get; private set; }
    public WeeklySchedule WeeklySchedule { get; private set; }
    public List<DoctorAvailabilityException> AvailabilityExceptions { get; private set; }

    private Doctor(){}
    
    private Doctor(
        string userId,
        string firstName,
        string lastName,
        string bio,
        List<Speciality> specialities,
        string timeZoneId,
        WeeklySchedule weeklySchedule,
        List<DoctorAvailabilityException> availabilityExceptions)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Bio = bio;
        Specialities = specialities;
        TimeZoneId = timeZoneId;
        WeeklySchedule = weeklySchedule;
        AvailabilityExceptions = availabilityExceptions;
    }

    public static Result<Doctor> Create(
        string userId,
        string firstName,
        string lastName,
        string bio,
        List<Speciality> specialities,
        string timeZoneId,
        WeeklySchedule? weeklySchedule = null,
        List<DoctorAvailabilityException>? availabilityExceptions = null)
    {
        if (!IsValidTimeZone(timeZoneId))
            return Result<Doctor>.Failure(ResponseList.InvalidTimezone);
        
        return  Result<Doctor>.Success(new Doctor(
            userId,
            bio,
            firstName,
            lastName,
            specialities,
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
    
    public Result RemoveUnavailability(DateTime start, DateTime end)
    {
        var exceptions = RemoveAvailabilityExceptions(
            DateTimeRange.Create(start, end),
            AvailabilityExceptionType.Unavailable);
        
        if (!exceptions.Any())
            return Result.Failure(ResponseList.ExceptionNotFound);

        foreach (var exception in exceptions)
            AvailabilityExceptions.Remove(exception);

        RaiseDomainEvent(new DoctorRemovedUnavailabilityDomainEvent(Id, start, end));
        return Result.Success();
        
    }

    public Result RemoveExtraAvailability(DateTime start, DateTime end)
    {
        var exceptions = RemoveAvailabilityExceptions(
            DateTimeRange.Create(start, end),
            AvailabilityExceptionType.ExtraAvailability);
        
        if (!exceptions.Any())
            return Result.Failure(ResponseList.ExceptionNotFound);

        foreach (var exception in exceptions)
            AvailabilityExceptions.Remove(exception);

        RaiseDomainEvent(new DoctorRemovedUnavailabilityDomainEvent(Id, start, end));
        return Result.Success();
        
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
    
    public void UpdateProfile(string? timeZoneId, string? bio)
    {
        if (!string.IsNullOrWhiteSpace(timeZoneId))
            TimeZoneId = timeZoneId;

        if (!string.IsNullOrWhiteSpace(bio))
            Bio = bio;
    }
    
    public void UpdateNames(string? firstName, string? lastName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;

        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;
    }

    public Result AddSpeciality(Speciality speciality)
    {
        var exists = Specialities.Exists(p => p.Name == speciality.Name);
        if (exists)
            return Result.Failure(ResponseList.SpecialityBelongsToDoctor);
        
        Specialities.Add(speciality);

        return Result.Success();
    }
    
    public Result RemoveSpeciality(string name)
    {
        var speciality = Specialities.Find(p => p.Name == name);
        if (speciality == null)
            return Result.Failure(ResponseList.SpecialityNotBelongDoctor);
        
        Specialities.Remove(speciality);

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
    
    private List<DoctorAvailabilityException> RemoveAvailabilityExceptions(DateTimeRange range, AvailabilityExceptionType type)
    {
        var overlapping = AvailabilityExceptions
            .Where(e => e.Type == type && e.Overlaps(range))
            .ToList();

        return overlapping;
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

