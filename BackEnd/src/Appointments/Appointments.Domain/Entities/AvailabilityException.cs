using Appointments.Domain.Entities.Enums;

namespace Appointments.Domain.Entities;

public class AvailabilityException
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }
    public string Reason { get; private set; }
    public ExceptionType Type { get; private set; }

    private AvailabilityException() { }

    private AvailabilityException(DateTime start, DateTime end, string reason, ExceptionType type)
    {
        Start = start;
        End = end;
        Reason = reason;
        Type = type;
    }

    public static AvailabilityException CreateExtraAvailability(DateTime start, DateTime end, string reason)
        => new(start, end, reason, ExceptionType.ExtraAvailability);

    public static AvailabilityException CreateUnavailability(DateTime start, DateTime end, string reason)
        => new(start, end, reason, ExceptionType.Unavailability);

    public bool Overlaps(DateTime start, DateTime end) =>
        Start < end && start < End;
}
