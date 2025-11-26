using FluentValidation.Results;
using Shared.Domain.Exceptions;

namespace Doctors.Domain.Entities;

public sealed class WorkTimeRange
{
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }

    private WorkTimeRange() { }

    public static WorkTimeRange Create(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
            throw new HamsValidationException(new[]
            {
                new ValidationFailure(
                    "WorkTimeRange", "Start time must be before end time")
            });

        return new WorkTimeRange
        {
            Start = start,
            End = end
        };
    }

    public bool Contains(TimeOnly timeOfDay) =>
        timeOfDay >= Start && timeOfDay < End;

    public bool Overlaps(WorkTimeRange other)
    {
        return Start < other.End && End > other.Start;
    }
}
