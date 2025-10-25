namespace Doctors.API.Doctors.Models.Responses;

public sealed record DoctorQueryResponse(
    string Id,
    string FirstName,
    string LastName,
    string UserId,
    string Bio,
    string TimeZoneId,
    List<string> Specialities,
    List<WorkDayResponse> WorkDays,
    List<DoctorAvailabilityExceptionResponse> AvailabilityExceptions);
