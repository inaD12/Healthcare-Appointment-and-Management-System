using Doctors.Application.Features.Doctors.Dtos;

namespace Doctors.API.Doctors.Models.Responses;

public sealed record DoctorQueryResponse(
    string Id,
    string UserId,
    string TimeZoneId,
    List<string> Specialities,
    List<WorkDayResponse> WorkDays,
    List<DoctorAvailabilityExceptionResponse> AvailabilityExceptions);
