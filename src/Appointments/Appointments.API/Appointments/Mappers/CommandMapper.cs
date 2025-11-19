using Appointments.API.Appointments.Models.Requests;
using Appointments.API.Appointments.Models.Responses;
using Appointments.Application.Features.Appointments.Commands.CancelAppointment;
using Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;

namespace Appointments.API.Appointments.Mappers;

public static class CommandMapper
{
    public static CreateAppointmentCommand ToCommand(
        this CreateAppointmentRequest request)
        => new(
            request.PatientEmail,
            request.DoctorEmail,
            request.ScheduledStartTime,
            request.Duration);
    
    public static CancelAppointmentCommand ToCommand(
        this CancelAppointmentRequest request)
        => new(
            request.AppointmentId);
    
    public static RescheduleAppointmentCommand ToCommand(
        this RescheduleAppointmentRequest request)
        => new(
            request.AppointmentID,
            request.ScheduledStartTime,
            request.Duration);
    
    public static AppointmentCommandResponse ToResponse(
        this AppointmentCommandViewModel request)
        => new(
            request.Id);
}
  