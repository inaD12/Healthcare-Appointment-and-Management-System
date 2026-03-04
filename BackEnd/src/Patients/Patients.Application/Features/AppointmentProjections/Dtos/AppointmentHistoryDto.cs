using Patients.Infrastructure.Features.ReadModels.Enums;

namespace Patients.Application.Features.AppointmentProjections.Dtos;

public sealed record AppointmentHistoryDto(
    string Id,
    DateTime Start,
    DateTime End,
    AppointmentStatus Status,
    string DoctorId,
    string PatientId);