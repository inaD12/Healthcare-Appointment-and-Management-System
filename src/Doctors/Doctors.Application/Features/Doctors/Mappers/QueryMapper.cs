using Doctors.Application.Features.Doctors.Dtos;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Entities;

namespace Doctors.Application.Features.Doctors.Mappers;

public static class QueryMapper
{
    public static DoctorQueryViewModel ToQueryViewModel(
        this Doctor doctor)
        => new(
            doctor.Id,
            doctor.UserId,
            doctor.TimeZoneId,
            doctor.Specialities.Select(s => s.ToString()).ToList(),
            doctor.WeeklySchedule.WorkDays.Select(s => s.ToDto()).ToList(),
            doctor.AvailabilityExceptions.Select(s => s.ToDto()).ToList());

    public static WorkDayDto ToDto(
        this WorkDay workDay)
        => new(
            workDay.DayOfWeek,
            workDay.WorkTimes.Select(s => s.ToDto()).ToList());
    
    public static WorkTimeRangeDto ToDto(
        this WorkTimeRange workTimeRange)
        => new(
            workTimeRange.Start,
            workTimeRange.End);
    
    public static DoctorAvailabilityExceptionDto ToDto(
        this DoctorAvailabilityException exception)
        => new(
            exception.Range.Start,
            exception.Range.End,
            exception.Reason,
            exception.Type);
}