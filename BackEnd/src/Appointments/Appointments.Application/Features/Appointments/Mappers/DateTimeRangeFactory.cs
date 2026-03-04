using Appointments.Domain.Entities.Enums;
using Shared.Domain.Entities.ValueObjects;

namespace Appointments.Application.Features.Appointments.Mappers;

public static class DateTimeRangeFactory
{
    public static DateTimeRange FromDuration(DateTime start, AppointmentDuration duration)
    {
        var end = start.AddMinutes((int)duration);
        return DateTimeRange.Create(start, end);
    }
}
