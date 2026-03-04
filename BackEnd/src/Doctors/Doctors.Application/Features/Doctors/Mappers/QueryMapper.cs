using Doctors.Application.Features.Doctors.Dtos;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Application.Features.Doctors.Queries.GetAllDoctors;
using Doctors.Domain.Entities;
using Doctors.Domain.Models;
using Shared.Domain.Models;

namespace Doctors.Application.Features.Doctors.Mappers;

public static class QueryMapper
{
    public static DoctorQueryViewModel ToQueryViewModel(
        this Doctor doctor)
        => new(
            doctor.Id,
            doctor.FirstName,
            doctor.LastName,
            doctor.UserId,
            doctor.Bio,
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

    public static DoctorPagedListQuery ToInfraQuery(
        this GetAllDoctorsQuery query)
        => new(
            query.FirstName,
            query.LastName,
            query.Speciality,
            query.TimeZoneId,
            query.SortOrder,
            query.SortPropertyName,
            query.Page,
            query.PageSize);
    
    public static DoctorPaginatedQueryViewModel ToViewModel(
        this PagedList<Doctor> pagedList)
        => new(
            pagedList.Items.Select(i => i.ToQueryViewModel()).ToList(),
            pagedList.Page,
            pagedList.PageSize,
            pagedList.TotalCount,
            pagedList.HasNextPage,
            pagedList.HasPreviousPage);
}