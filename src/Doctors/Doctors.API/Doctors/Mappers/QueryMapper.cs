using Doctors.API.Doctors.Models.Requests;
using Doctors.API.Doctors.Models.Responses;
using Doctors.Application.Features.Doctors.Dtos;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Application.Features.Doctors.Queries.GetAllDoctors;

namespace Doctors.API.Doctors.Mappers;

public static class QueryMapper
{
    public static DoctorQueryResponse ToResponse(
        this DoctorQueryViewModel doctor)
        => new(
            doctor.Id,
            doctor.FirstName,
            doctor.LastName,
            doctor.UserId,
            doctor.Bio,
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
    
    public static GetAllDoctorsQuery ToQuery(
        this GetAllDoctorsRequest request)
        => new(
            request.FirstName,
            request.LastName,
            request.Speciality,
            request.TimeZoneId,
            request.SortOrder,
            request.Page,
            request.PageSize,
            request.SortPropertyName);
    
    public static DoctorPaginatedQueryResponse ToResponse(
        this DoctorPaginatedQueryViewModel viewModel)
        => new(
            viewModel.Items,
            viewModel.Page,
            viewModel.PageSize,
            viewModel.TotalCount,
            viewModel.HasNextPage,
            viewModel.HasPreviousPage);
}