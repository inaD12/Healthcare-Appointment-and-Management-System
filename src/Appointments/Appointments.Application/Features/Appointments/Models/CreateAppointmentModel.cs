using Appointments.Domain.Enums;

namespace Appointments.Application.Features.Appointments.Models;

public record CreateAppointmentModel(string DoctorId, string PatientId, DateTime StartTime, AppointmentDuration Duration, AppointmentStatus Status);
