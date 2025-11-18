using Appointments.API.Appointments.Models.Requests;
using Appointments.API.Appointments.Models.Responses;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;

namespace Appointments.API.Appointments.Mappers;

public static class QueryMapper
{
    public static AppointmentQueryResponse ToResponse(
        this AppointmentQueryViewModel viewModel)
        => new(
            viewModel.Id,
            viewModel.PatientId,
            viewModel.DoctorId,
            viewModel.Duration,
            viewModel.Status);

    public static GetAllAppointmentsQuery ToQuery(
        this GetAllAppointmentsRequest request)
        => new(
            request.PatientId,
            request.DoctorId,
            request.Status,
            request.FromTime,
            request.ToTime,
            request.SortOrder,
            request.SortPropertyName,
            request.Page,
            request.PageSize);
    
    public static AppointmentPaginatedQueryResponse ToResponse(
        this AppointmentPaginatedQueryViewModel viewModel)
        => new(
            viewModel.Items,
            viewModel.Page,
            viewModel.PageSize,
            viewModel.TotalCount,
            viewModel.HasNextPage,
            viewModel.HasPreviousPage);
}