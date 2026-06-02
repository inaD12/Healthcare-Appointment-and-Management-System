using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Enums;
using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(
string PatientUserId,
string DoctorUserId,
DateTime ScheduledStartTime,
AppointmentDuration Duration) : ICommand<AppointmentCommandViewModel>;
