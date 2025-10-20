using Doctors.API.Doctors.Models.Responses;
using Doctors.Application.Features.Doctors.Dtos;
using Doctors.Application.Features.Doctors.Models;

namespace Doctors.API.Doctors.Mappers;

public static class QueryMapper
{
    public static DoctorQueryResponse ToResponse(
        this DoctorQueryViewModel doctor)
        => new(
            doctor.Id,
            doctor.UserId,
            doctor.TimeZoneId,
            doctor.Specialities.Select(s => s.ToString()).ToList(),
            doctor.WorkDays.Select(s => s.ToResponse()).ToList(),
            doctor.AvailabilityExceptions.Select(s => s.ToResponse()).ToList());

    public static WorkDayResponse ToResponse(
        this WorkDayDto workDay)
        => new(
            workDay.DayOfWeek,
            workDay.WorkTimes.Select(s => s.ToResponse()).ToList());
    
    public static WorkTimeRangeResponse ToResponse(
        this WorkTimeRangeDto workTimeRange)
        => new(
            workTimeRange.Start,
            workTimeRange.End);
    
    public static DoctorAvailabilityExceptionResponse ToResponse(
        this DoctorAvailabilityExceptionDto exception)
        => new(
            exception.Start,
            exception.End,
            exception.Reason,
            exception.Type);
}