using Doctors.Domain.Entities;

namespace Doctors.Application.Features.Doctors.Dtos;

public sealed record DoctorAvailabilityExceptionDto(DateTime Start, DateTime End, string Reason, AvailabilityExceptionType Type);