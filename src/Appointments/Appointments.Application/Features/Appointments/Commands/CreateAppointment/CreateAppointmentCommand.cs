using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Commands.Appointments.CreateAppointment;

public sealed record CreateAppointmentCommand(
string PatientEmail,
string DoctorEmail,
DateTime ScheduledStartTime,
AppointmentDuration Duration) : ICommand<AppointmentCommandViewModel>;
