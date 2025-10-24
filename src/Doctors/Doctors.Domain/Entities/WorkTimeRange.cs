using Shared.Domain.Exceptions;

namespace Doctors.Domain.Entities;

public sealed class WorkTimeRange
{
    public TimeSpan Start { get; init; }
    public TimeSpan End { get; init; }

    private WorkTimeRange() { }

    public static WorkTimeRange Create(TimeSpan start, TimeSpan end)
    {
        if (start >= end)
            throw new HAMSValidationException("WorkTimeRange", "Start time must be before end time");

        return new WorkTimeRange
        {
            Start = start,
            End = end
        };
    }

    public bool Contains(TimeSpan timeOfDay) =>
        timeOfDay >= Start && timeOfDay < End;

    public bool Overlaps(WorkTimeRange other)
    {
        return Start < other.End && End > other.Start;
    }
}
