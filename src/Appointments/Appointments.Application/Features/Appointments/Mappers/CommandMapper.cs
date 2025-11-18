using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;

namespace Appointments.Application.Features.Appointments.Mappers;

public static class CommandMapper
{
    public static AppointmentCommandViewModel ToCommandViewModel(
        this Appointment appointment)
        => new(
            appointment.Id);
}
  