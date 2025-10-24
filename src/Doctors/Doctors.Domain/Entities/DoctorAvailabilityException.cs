using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Exceptions;

namespace Doctors.Domain.Entities;

public sealed class DoctorAvailabilityException
{
    public DateTimeRange Range { get; init; }
    public AvailabilityExceptionType Type { get; init; }
    public string Reason { get; init; }

    private DoctorAvailabilityException() { }

    public static DoctorAvailabilityException Create(
        DateTime start,
        DateTime end,
        AvailabilityExceptionType type,
        string reason = "")
    {
        if (start >= end)
            throw new HAMSValidationException(nameof(DoctorAvailabilityException), "Start must be before end");

        return new DoctorAvailabilityException
        {
            Range = DateTimeRange.Create(start, end),
            Type = type,
            Reason = reason
        };
    }

    public bool Overlaps(DateTimeRange other) =>
        Range.Start < other.End && other.Start < Range.End;

    public bool Contains(DateTime dateTimeUtc) =>
        Range.Start <= dateTimeUtc && dateTimeUtc < Range.End;
}

public enum AvailabilityExceptionType
{
    Unavailable = 0,
    ExtraAvailability = 1
}
