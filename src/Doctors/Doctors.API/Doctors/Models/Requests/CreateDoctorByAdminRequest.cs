namespace Doctors.API.Doctors.Models.Requests;

public sealed record CreateDoctorByAdminRequest(
    string UserId,
    List<string> Specialities,
    string TimeZoneId);
