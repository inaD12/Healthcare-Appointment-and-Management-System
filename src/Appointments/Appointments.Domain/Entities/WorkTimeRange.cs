using FluentValidation.Results;
using Shared.Domain.Exceptions;

namespace Appointments.Domain.Entities;

public sealed class WorkTimeRange
{
    public TimeSpan Start { get; init; }
    public TimeSpan End { get; init; }

    private WorkTimeRange() { }

    public static WorkTimeRange Create(TimeSpan start, TimeSpan end)
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
}
