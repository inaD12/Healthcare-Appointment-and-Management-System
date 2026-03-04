using FluentValidation.Results;
using Shared.Domain.Exceptions;

namespace Doctors.Domain.Entities;

public sealed class WorkDay
{
    public DayOfWeek DayOfWeek { get; init; }

    private List<WorkTimeRange> _workTimes = new();
    public IReadOnlyCollection<WorkTimeRange> WorkTimes => _workTimes.AsReadOnly();

    private WorkDay() { }

    public static WorkDay Create(DayOfWeek dayOfWeek, IEnumerable<WorkTimeRange> workTimes)
    {
        if (workTimes == null || !workTimes.Any())
            throw new HamsValidationException(new[]
            {
                new ValidationFailure(
                    "WorkDay", "At least one work time is required")
            });

        var sortedTimes = workTimes.OrderBy(w => w.Start).ToList();

        for (int i = 1; i < sortedTimes.Count; i++)
        {
            var prev = sortedTimes[i - 1];
            var current = sortedTimes[i];

            if (prev.Overlaps(current))
                throw new HamsValidationException(new[]
                {
                    new ValidationFailure(
                        "WorkDay",
                        $"Overlapping work time ranges on {dayOfWeek}: {prev.Start}-{prev.End} and {current.Start}-{current.End}")
                });
        }

        return new WorkDay
        {
            DayOfWeek = dayOfWeek,
            _workTimes = sortedTimes
        };
    }

    public bool IsWithinWorkingHours(TimeOnly timeOfDay)
        => _workTimes.Any(range => range.Contains(timeOfDay));
}
