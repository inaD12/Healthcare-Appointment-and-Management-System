namespace Doctors.API.Doctors.Models.Requests;

public sealed record CreateDoctorRequest(
    List<string> Specialities,
    string TimeZoneId );