using Doctors.Domain.Entities;

namespace Doctors.API.Doctors.Models.Responses;

public sealed record DoctorAvailabilityExceptionResponse(DateTime Start, DateTime End, string Reason, AvailabilityExceptionType Type);