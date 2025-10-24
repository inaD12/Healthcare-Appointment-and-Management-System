namespace Doctors.API.Doctors.Models.Requests;

public sealed record CreateDoctorByAdminRequest(
    string UserId,
    string Bio,
    List<string> Specialities,
    string TimeZoneId);
