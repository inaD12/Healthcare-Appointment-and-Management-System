namespace Doctors.API.Doctors.Models.Requests;

public sealed record CreateDoctorRequest(
    string Bio,
    List<string> Specialities,
    string TimeZoneId );