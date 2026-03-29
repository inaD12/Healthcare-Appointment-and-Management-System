using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using Shared.Domain.Models;

namespace Appointments.Application.Features.Appointments.Mappers;

public static class QueryMapper
{
    public static AppointmentQueryViewModel ToQueryViewModel(
        this Appointment appointment)
        => new(
            appointment.Id,
            appointment.PatientId,
            appointment.DoctorId,
            appointment.Duration,
            appointment.Status);

    public static AppointmentPagedListQuery ToPagedListQuery(
        this GetAllAppointmentsQuery query)
        => new(
            query.PatientId ?? null,
            query.DoctorId,
            query.Status,
            query.FromTime,
            query.ToTime,
            query.SortOrder,
            query.SortPropertyName,
            query.Page,
            query.PageSize);
    
    public static AppointmentPaginatedQueryViewModel ToViewModel(
        this PagedList<Appointment> appointments)
        => new(
            appointments.Items.Select(a => a.ToQueryViewModel()).ToList(),
            appointments.Page,
            appointments.PageSize,
            appointments.TotalCount,
            appointments.HasNextPage,
            appointments.HasPreviousPage
            );
    
    public static ICollection<BookingQueryViewModel> ToBookingQueryViewModelCollection(
        this List<Appointment> appointments)
        => appointments.Select(a => a.ToBookingQueryViewModel()).ToList();
    
    public static ICollection<AppointmentQueryViewModel> ToAppointmentsQueryViewModelCollection(
        this List<Appointment> appointments)
        => appointments.Select(a => a.ToQueryViewModel()).ToList();
    
    public static BookingQueryViewModel ToBookingQueryViewModel(
        this Appointment appointment)
        => new(
            appointment.Duration.Start,
            appointment.Duration.End);
}