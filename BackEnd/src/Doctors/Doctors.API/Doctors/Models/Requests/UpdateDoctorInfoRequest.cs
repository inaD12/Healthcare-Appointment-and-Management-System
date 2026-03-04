namespace Doctors.API.Doctors.Models.Requests;

public sealed record UpdateDoctorInfoRequest(
    string? NewBio,
    string? NewTimeZoneId );