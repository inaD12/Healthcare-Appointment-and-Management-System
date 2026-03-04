namespace Doctors.API.Doctors.Models.Requests;

public sealed record UpdateDoctorInfoByAdminRequest(
    string UserId,
    string? NewBio,
    string? NewTimeZoneId );