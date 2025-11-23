using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(
string PatientUserId,
string DoctorUserId,
DateTime ScheduledStartTime,
AppointmentDuration Duration) : ICommand<AppointmentCommandViewModel>;
