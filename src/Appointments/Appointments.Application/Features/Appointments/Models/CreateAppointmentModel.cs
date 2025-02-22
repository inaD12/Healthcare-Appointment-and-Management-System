using Appointments.Domain.Enums;

namespace Appointments.Application.Features.Appointments.Models;

public record CreateAppointmentModel(string doctorId, string patientId, DateTime startTime, AppointmentDuration duration);
